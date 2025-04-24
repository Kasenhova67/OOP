
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
    public class DocumentManager
    {

        private Document _currentDocument;
        private readonly Dictionary<string, IDocumentAdapter> _adapters = new Dictionary<string, IDocumentAdapter>
        {
            { "txt", new PlainTextAdapter() },
            { "json", new JsonAdapter() },
            { "xml", new XmlAdapter() }
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

        public Document CurrentDocument => _currentDocument;

        public void CreateDocument(string type = "plain")
        {
            if (_factories.TryGetValue(type.ToLower(), out var factory))
            {
                _currentDocument = factory.CreateDocument();
                _currentDocument.Title = $"New {type} Document";
                Console.WriteLine($"Created new {type} document");
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
                    string content = adapter.Convert(_currentDocument);
                    storage.Save(path, content);
                    Console.WriteLine($"Document saved to {path}");
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

        public void ShowAvailableFormats()
        {
            Console.WriteLine("Available formats:");
            foreach (var format in _adapters.Keys)
            {
                Console.WriteLine($"- {format}");
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
    }
}
