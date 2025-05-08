
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json; 

namespace testt
{
    public class DocumentManager
    {
         private Document _currentDocument;
            private readonly Dictionary<string, IDocumentAdapter> _adapters = new Dictionary<string, IDocumentAdapter>
        {
            { "txt", new PlainTextAdapter() },
            { "json", new JsonAdapter() },
            { "xml", new XmlAdapter() },
            { "md", new MarkdownAdapter() },
            { "rtf", new RichTextAdapter() }
        };

            private readonly Dictionary<string, IStorageStrategy> _storageStrategies = new Dictionary<string, IStorageStrategy>
        {
            { "local", new LocalFileStorage() },
            { "onedrive", new OneDriveLocalStorage() }
        };

            private readonly Dictionary<string, IDocumentFactory> _factories = new Dictionary<string, IDocumentFactory>
        {
            { "plain", new PlainTextFactory() },
            { "markdown", new MarkdownFactory() },
            { "richtext", new RichTextFactory() }
        };

            private readonly Dictionary<string, DocumentHistory> _documentHistories = new Dictionary<string, DocumentHistory>();

            public Document CurrentDocument => _currentDocument;

            public void CreateDocument(string type, string path)
            {
                if (_factories.TryGetValue(type.ToLower(), out var factory))
                {
                    _currentDocument = factory.CreateDocument();
                    _currentDocument.Title = Path.GetFileNameWithoutExtension(path);
                    Console.WriteLine($"Created new {type} document: {path}");

                    var owner = UserManager.Instance.GetCurrentUser();
                    if (owner != null)
                    {
                        owner.OwnedDocuments.Add(path);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid document type");
                }
            }

        public void LoadDocument(string path, string storageType = "local")
        {


            try
            {
                if (_storageStrategies.TryGetValue(storageType.ToLower(), out var storage))
                {
                    string content = storage.Load(path);
                    string extension = Path.GetExtension(path).TrimStart('.').ToLower();

                    if (_adapters.TryGetValue(extension, out var adapter))
                    {
                        _currentDocument = adapter.ConvertBack(content);
                        _currentDocument.Title = Path.GetFileNameWithoutExtension(path);
                        Console.WriteLine($"Document loaded from {path}");

                        var currentUser = UserManager.Instance.GetCurrentUser();
                        if (currentUser != null && !currentUser.HasAccess(path, false))
                        {
                            Console.WriteLine("Error: You don't have access to this document");
                            return;
                        }
                        if (_currentDocument != null)
                        {
                            _currentDocument.Attach(currentUser);
                        }

                    }
                    else
                    {
                        Console.WriteLine("Unsupported file format");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid storage type");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading document: {ex.Message}");
            }
        }



        public void SaveDocument(string path, string storageType = "local")
        {
            if (_currentDocument == null)
            {
                Console.WriteLine("No document to save");
                return;
            }

            try
            {
                string extension = Path.GetExtension(path).TrimStart('.').ToLower();

                if (_adapters.TryGetValue(extension, out var adapter) &&
                    _storageStrategies.TryGetValue(storageType.ToLower(), out var storage))
                {
                    var currentUser = UserManager.Instance.GetCurrentUser();
                    if (currentUser != null && !currentUser.HasAccess(path, true))
                    {
                        Console.WriteLine("Error: You don't have edit access to this document");
                        return;
                    }

                    string content = adapter.Convert(_currentDocument);
                    storage.Save(path, content);
                    Console.WriteLine($"Document saved to {path}");

                    // Update document title
                    _currentDocument.Title = Path.GetFileNameWithoutExtension(path);

                    // Commit notifications for this document
                    _currentDocument.NotificationService.CommitChanges(_currentDocument.Title);

                    // Save user data
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        IncludeFields = true
                    };
                    var users = UserManager.Instance.GetAllUsers();
                    string json = JsonSerializer.Serialize(users, options);
                    File.WriteAllText("users.json", json);
                }
                else
                {
                    Console.WriteLine("Unsupported file format or storage type");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving document: {ex.Message}");
            }
        }

        public void AddHistoryEntry(string path, string user, string description)
            {
                if (!_documentHistories.TryGetValue(path, out var history))
                {
                    history = new DocumentHistory(path);
                    _documentHistories[path] = history;
                }
                history.AddChange(user, description);
            }

            
            public void ShowDocumentHistory(string path)
            {
                if (_documentHistories.TryGetValue(path, out var history))
                {
                    Console.WriteLine($"History for document: {path}");
                    Console.WriteLine($"Owner: {AccessManager.Instance.GetDocumentOwner(path) ?? "Unknown"}");
                    history.DisplayHistory();
                }
                else
                {
                    Console.WriteLine("No history available for this document");
                }
            }

            public void ShowAvailableStorageTypes()
            {
                Console.WriteLine("Available storage types:");
                foreach (var type in _storageStrategies.Keys)
                {
                    Console.WriteLine($"- {type}");
                }
            }

            public void ShowAvailableDocumentTypes()
            {
                Console.WriteLine("Available document types:");
                foreach (var type in _factories.Keys)
                {
                    Console.WriteLine($"- {type}");
                }
            }
      

        public Document GetDocumentByPath(string path)
        {
            
            if (_currentDocument != null && _currentDocument.Title.Equals(path, StringComparison.OrdinalIgnoreCase))
            {
                return _currentDocument;
            }
            return null;
        }



    }
}
