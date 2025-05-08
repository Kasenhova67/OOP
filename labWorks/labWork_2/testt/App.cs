using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static testt.AccessManager;

namespace testt
{
    public class DocumentEditorApp
    {
        private readonly DocumentManager _documentManager = new DocumentManager();
        private User _currentUser;
        private bool _previewMode = false;
        private Thread _inputThread;
        private bool _exitRequested = false;

        private void SubscribeToCurrentDocument()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _currentUser.SubscribeToDocument(_documentManager.CurrentDocument);
        }

        private void ShowAllNotifications()
        {
            var notifications = _currentUser.GetNotifications();
            DisplayNotifications(notifications, "All notifications");
            _currentUser.ClearNotifications();
        }
        private void UnsubscribeFromCurrentDocument()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _currentUser.UnsubscribeFromDocument(_documentManager.CurrentDocument);
        }

        private void NotificationMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("Notification Menu:");
                Console.WriteLine("1. View all notifications");
                Console.WriteLine("2. Subscribe to current document");
                Console.WriteLine("3. Unsubscribe from current document");
                Console.WriteLine("4. Check subscription status");
                Console.WriteLine("5. Back to main menu");
                Console.Write("Choose an option: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": ShowAllNotifications(); break;
                    case "2": SubscribeToCurrentDocument(); break;
                    case "3": UnsubscribeFromCurrentDocument(); break;
                    case "4": CheckSubscriptionStatus(); break;
                    case "5": back = true; break;
                   
                    default: ShowError("Invalid option"); break;
                }
            }
        }

        private void CheckSubscriptionStatus()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            bool isSubscribed = _currentUser.SubscribedDocuments.Contains(
                _documentManager.CurrentDocument.Title);

            Console.WriteLine($"You are {(isSubscribed ? "" : "not ")}subscribed to this document");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        private void MarkAllNotificationsAsRead()
        {
            _currentUser.MarkAllNotificationsAsRead();
            Console.WriteLine("All notifications marked as read");
            Thread.Sleep(1000);
        }


        private void ShowDocumentNotifications()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            var notifications = _currentUser.GetNotifications()
                .Where(n => n.DocumentPath == _documentManager.CurrentDocument.Title)
                .ToList();

            DisplayNotifications(notifications, $"Notifications for: {_documentManager.CurrentDocument.Title}");
        }

        private void MarkNotificationsAsRead()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _currentUser.MarkNotificationsAsRead(_documentManager.CurrentDocument.Title);
            Console.WriteLine("Notifications marked as read");
            Thread.Sleep(1000);
        }

        private void LoginUser()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            User existingUser = UserManager.Instance.GetUser(name);

            if (existingUser != null)
            {
                _currentUser = existingUser;
                UserManager.Instance.SetCurrentUser(_currentUser);
                Console.WriteLine($"Welcome back {_currentUser.Name} ({_currentUser.Role})");
            }
            else
            {
                Console.WriteLine("Select your role (1-3):");
                Console.WriteLine("1. Viewer (read-only)");
                Console.WriteLine("2. Editor (can edit documents)");
                Console.WriteLine("3. Admin (full access)");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int roleChoice) && roleChoice >= 1 && roleChoice <= 3)
                {
                    _currentUser = new User(name, (UserRole)(roleChoice - 1));
                    UserManager.Instance.AddUser(_currentUser);
                    UserManager.Instance.SetCurrentUser(_currentUser);
                    Console.WriteLine($"Logged in as {_currentUser.Name} ({_currentUser.Role})");
                }
                else
                {
                    Console.WriteLine("Invalid choice, defaulting to Viewer role");
                    _currentUser = new User(name, UserRole.Viewer);
                    UserManager.Instance.AddUser(_currentUser);
                    UserManager.Instance.SetCurrentUser(_currentUser);
                }
            }
        }
        private void DisplayNotifications(List<UserNotification> notifications, string title)
        {
            Console.Clear();
            Console.WriteLine($"\n{title}");
            Console.WriteLine("----------------------------------");

            if (notifications.Count == 0)
            {
                Console.WriteLine("No notifications");
            }
            else
            {
                foreach (var notification in notifications.OrderByDescending(n => n.Timestamp))
                {
                    Console.ForegroundColor = notification.IsRead ? ConsoleColor.Gray : ConsoleColor.White;
                    Console.WriteLine($"[{notification.Timestamp}] {notification.Message}");
                    Console.WriteLine($"Document: {notification.DocumentPath}");
                    Console.WriteLine("----------------------------------");
                }
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

       
        private void MarkCurrentDocNotificationsAsRead()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _currentUser.MarkDocumentNotificationsAsRead(_documentManager.CurrentDocument.Title);
            Console.WriteLine("Notifications marked as read");
            Thread.Sleep(1000);
        }

      
       /* private void SubscribeToDocument()
        {
            Console.Write("Enter the path to your document: ");
            string path = Console.ReadLine();

            var doc = _documentManager.GetDocumentByPath(path);
            if (doc != null)
            {
                _currentUser.SubscribeToDocument(doc);
                Console.WriteLine($"You subscribed on updates of: {path}");
            }
            else
            {
                ShowError("Not found");
            }

            Thread.Sleep(1000);
        }

        private void UnsubscribeFromDocument()
        {
            Console.Write("Enter the path: ");
            string path = Console.ReadLine();

            var doc = _documentManager.GetDocumentByPath(path);
            if (doc != null)
            {
                _currentUser.UnsubscribeFromDocument(doc);
                Console.WriteLine($"You unsubscribed from: {path}");
            }
            else
            {
                ShowError("Not found");
            }

            Thread.Sleep(1000);
        }*/

        private void DisplayMainMenu()
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Document Operations");
            Console.WriteLine("2. User Settings");
            Console.WriteLine("3. Application Settings");
            Console.WriteLine("4. View Document History");
            Console.WriteLine("5. Manage Document Access");
            Console.WriteLine("6. Notifications");
            Console.WriteLine("7. Exit");
            Console.Write("Your choice: ");
        }

        private void DocumentOperations()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\nDocument Operations:");
                Console.WriteLine("1. Create New Document");
                Console.WriteLine("2. Open Document");
                Console.WriteLine("3. Save Document");
                Console.WriteLine("4. Edit Document");
                Console.WriteLine("5. View Document");
                Console.WriteLine("6. View in Preview Mode");
                Console.WriteLine("7. Search in Document");
                Console.WriteLine("8. Back to Main Menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int docChoice))
                {
                    switch (docChoice)
                    {
                        case 1: CreateDocument(); break;
                        case 2: OpenDocument(); break;
                        case 3: SaveDocument(); break;
                        case 4: EditDocument(); break;
                        case 5: ViewDocument(); break;
                        case 6: ViewPreviewMode(); break;
                        case 7: SearchDocument(); break;
                        case 8: back = true; break;
                        default: ShowError("Invalid choice"); break;
                    }
                }
                else
                {
                    ShowError("Please enter a number");
                }
            }
        }

        private void EditDocument()

        {

            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            string currentPath = _documentManager.CurrentDocument.Title;

            if (!_currentUser.HasAccess(currentPath, true))
            {
                ShowError("You don't have edit access to this document");
                return;
            }


            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\nEdit Document:");
                Console.WriteLine("1. Insert Text");
                Console.WriteLine("2. Delete Text");
                Console.WriteLine("3. Format Text");
                Console.WriteLine("4. Copy Text");
                Console.WriteLine("5. Cut Text");
                Console.WriteLine("6. Paste Text");
                Console.WriteLine("7. Undo");
                Console.WriteLine("8. Redo");
                Console.WriteLine("9. Back to Document Menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int editChoice))
                {
                    switch (editChoice)
                    {
                        case 1: InsertText(); break;
                        case 2: DeleteText(); break;
                        case 3: FormatText(); break;
                        case 4: CopyText(); break;
                        case 5: CutText(); break;
                        case 6: PasteText(); break;
                        case 7: _documentManager.CurrentDocument.Undo(_currentUser.Name); break;
                        case 8: _documentManager.CurrentDocument.Redo(_currentUser.Name); break;
                        case 9: back = true; break;
                        default: ShowError("Invalid choice"); break;
                    }
                }
                else
                {
                    ShowError("Please enter a number");
                }
            }
        }
        private void ManageAccess()
        {
            Console.Write("Enter document path: ");
            string path = Console.ReadLine();

            var currentUser = UserManager.Instance.GetCurrentUser();
            string owner = AccessManager.Instance.GetDocumentOwner(path);

            if (owner != currentUser.Name && currentUser.Role != UserRole.Admin)
            {
                Console.WriteLine("Error: Only owner or admin can manage access");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine($"\nAccess Management for: {path}");
                Console.WriteLine($"Owner: {owner ?? "Unknown"}");
                Console.WriteLine("1. Grant access");
                Console.WriteLine("2. Revoke access");
                Console.WriteLine("3. View access list");
                Console.WriteLine("4. Back to main menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter username to grant access: ");
                            string grantUser = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(grantUser))
                            {
                                Console.WriteLine("Error: Username cannot be empty");
                                break;
                            }

                            Console.WriteLine("Select access level:");
                            Console.WriteLine("1. View only");
                            Console.WriteLine("2. Edit");
                            Console.Write("Your choice: ");

                            if (int.TryParse(Console.ReadLine(), out int accessChoice) && accessChoice >= 1 && accessChoice <= 2)
                            {
                                var level = accessChoice == 1 ?
                                    AccessManager.AccessLevel.View :
                                    AccessManager.AccessLevel.Edit;

                                AccessManager.Instance.GrantAccess(path, currentUser.Name, grantUser, level);
                                Console.WriteLine($"Access {level} granted to {grantUser} for document {path}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid access level selection");
                            }
                            break;

                        case 2:
                            Console.Write("Enter username to revoke access: ");
                            string revokeUser = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(revokeUser))
                            {
                                Console.WriteLine("Error: Username cannot be empty");
                                break;
                            }

                            AccessManager.Instance.RevokeAccess(path, revokeUser);
                            Console.WriteLine($"Access revoked from {revokeUser} for document {path}");
                            break;

                        case 3:
                            var accessList = AccessManager.Instance.GetAccessList(path);
                            Console.WriteLine($"\nAccess list for document: {path}");

                            if (accessList.Count == 0)
                            {
                                Console.WriteLine("No additional access rights granted");
                            }
                            else
                            {
                                Console.WriteLine("Users with access:");
                                foreach (var entry in accessList)
                                {
                                    Console.WriteLine($"- {entry.Key}: {entry.Value}");
                                }
                            }
                            break;

                        case 4:
                            back = true;
                            continue; // Пропускаем паузу при выходе

                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }

                    // Пауза после выполнения действия (кроме выхода)
                    if (choice != 4)
                    {
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private void UserSettings()
        {
            if (!_currentUser.CanManageUsers())
            {
                ShowError("You don't have permission to manage users");
                return;
            }

            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\nUser Settings:");
                Console.WriteLine("1. Change User Role");
                Console.WriteLine("2. List All Users");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            Console.Write("Enter user name: ");
                            string userName = Console.ReadLine();
                            Console.WriteLine("Select new role (1-3):");
                            Console.WriteLine("1. Viewer");
                            Console.WriteLine("2. Editor");
                            Console.WriteLine("3. Admin");
                            Console.Write("Your choice: ");
                            if (int.TryParse(Console.ReadLine(), out int newRole) && newRole >= 1 && newRole <= 3)
                            {
                                UserManager.Instance.UpdateUserRole(userName, (UserRole)(newRole - 1));
                                Console.WriteLine($"User {userName} role changed to {(UserRole)(newRole - 1)}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid role selection");
                            }
                            break;
                        case 2:
                            Console.WriteLine("Registered users:");
                            foreach (var user in UserManager.Instance.GetAllUsers())
                            {
                                Console.WriteLine($"- {user.Name} ({user.Role})");
                            }
                            break;
                        case 3:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        private void AppSettingsMenu()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\nApplication Settings:");
                Console.WriteLine("1. Change Text Color");
                Console.WriteLine("2. Change Background Color");
                /*    Console.WriteLine("3. Change Theme");*//*
                    Console.WriteLine("4. Change Font Size");*/
                Console.WriteLine("3. View Current Settings");
                Console.WriteLine("4. Back to Main Menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int settingsChoice))
                {
                    switch (settingsChoice)
                    {
                        case 1:
                            Console.WriteLine("Available colors:");
                            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                            {
                                Console.WriteLine($"- {color}");
                            }
                            Console.Write("Enter new text color: ");
                            if (Enum.TryParse(Console.ReadLine(), true, out ConsoleColor textColor))
                            {
                                AppSettings.Instance.TextColor = textColor;
                                AppSettings.Instance.ApplySettings();
                                Console.WriteLine("Text color changed");
                            }
                            else
                            {
                                Console.WriteLine("Invalid color");
                            }
                            break;
                        case 2:
                            Console.WriteLine("Available colors:");
                            foreach (ConsoleColor color in Enum.GetValues(typeof(ConsoleColor)))
                            {
                                Console.WriteLine($"- {color}");
                            }
                            Console.Write("Enter new background color: ");
                            if (Enum.TryParse(Console.ReadLine(), true, out ConsoleColor bgColor))
                            {
                                AppSettings.Instance.BackgroundColor = bgColor;
                                AppSettings.Instance.ApplySettings();
                                Console.WriteLine("Background color changed");
                            }
                            else
                            {
                                Console.WriteLine("Invalid color");
                            }
                            break;
                        /*    case 3:
                                Console.Write("Enter new theme name: ");
                                AppSettings.Instance.Theme = Console.ReadLine();
                                Console.WriteLine("Theme changed (visual changes may require restart)");
                                break;
                            case 4:
                                Console.Write("Enter new font size (8-24): ");
                                if (int.TryParse(Console.ReadLine(), out int fontSize) && fontSize >= 8 && fontSize <= 24)
                                {
                                    AppSettings.Instance.FontSize = fontSize;
                                    Console.WriteLine("Font size changed (may require restart)");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid font size");
                                }
                                break;*/
                        case 3:
                            Console.Clear();
                            Console.WriteLine("\nCurrent Settings:");
                            Console.WriteLine($"Text Color: {AppSettings.Instance.TextColor}");
                            Console.WriteLine($"Background Color: {AppSettings.Instance.BackgroundColor}");
                            Console.WriteLine($"Theme: {AppSettings.Instance.Theme}");
                            Console.WriteLine($"Font Size: {AppSettings.Instance.FontSize}");
                            break;
                        case 4:
                            back = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a number");
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
        public void Run()

        {
            try
            {
                AccessManager.Instance.LoadAccessData();
                UserManager.Instance.LoadUsers(); 
                AppSettings.Instance.ApplySettings();

                Console.WriteLine("Welcome to Console Document Editor!");
                

                LoginUser();
                StartInputThread();

                bool exit = false;
                while (!exit)
                {
                    Console.Clear();
                    DisplayMainMenu();

                    if (int.TryParse(Console.ReadLine(), out int mainChoice))
                    {
                        switch (mainChoice)
                        {
                            case 1: DocumentOperations(); break;
                            case 2: UserSettings(); break;
                            case 3: AppSettingsMenu(); break;
                            case 4: ShowHistory(); break;
                            case 5: ManageAccess(); break;
                            case 6: NotificationMenu(); break;
                            case 7: exit = true; _exitRequested = true; break;
                            default: ShowError("Invalid choice"); break;
                        }
                    }
                    else
                    {
                        ShowError("Please enter a number");
                    }
                }

                _inputThread.Join(100);
                Console.WriteLine("Goodbye!");
            }
            finally
            {
                UserManager.Instance.SaveUsers();
            }
        }
       
        private void StartInputThread()
        {
            _inputThread = new Thread(CheckForPreviewMode)
            {
                IsBackground = true
            };
            _inputThread.Start();
        }

        private void CheckForPreviewMode()
        {
            while (!_exitRequested)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.P &&
                    (ConsoleModifiers.Control & ConsoleModifiers.Control) != 0)
                {
                    TogglePreviewMode();
                    while (!_exitRequested && Console.ReadKey(true).Key != ConsoleKey.P ||
                          (ConsoleModifiers.Control & ConsoleModifiers.Control) == 0)
                    {
                        Thread.Sleep(50);
                    }
                    if (!_exitRequested)
                    {
                        TogglePreviewMode();
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void TogglePreviewMode()
        {
            if (_documentManager.CurrentDocument == null) return;

            _previewMode = !_previewMode;
            Console.Clear();

            if (_previewMode)
            {
                _documentManager.CurrentDocument.DisplayPreview();
                Console.WriteLine("\n[Press Ctrl+P to exit preview mode]");
            }
            else
            {
                _documentManager.CurrentDocument.DisplayEdit();
            }
        }

       

        private void ViewPreviewMode()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _documentManager.CurrentDocument.DisplayPreview();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void ViewDocument()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _documentManager.CurrentDocument.Display();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void SaveDocument()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            string currentPath = _documentManager.CurrentDocument.Title;

            if (!AccessManager.Instance.HasAccess(currentPath, _currentUser.Name, true))
            {
                ShowError("You don't have permission to save this document");
                return;
            }

            Console.Write($"Enter file path [current: {currentPath}]: ");
            string savePath = Console.ReadLine();
            if (string.IsNullOrEmpty(savePath))
            {
                savePath = currentPath;
            }

            _documentManager.ShowAvailableStorageTypes();
            Console.Write("Enter storage type (local/onedrive): ");
            string saveStorageType = Console.ReadLine().ToLower();

            try
            {
                _documentManager.SaveDocument(savePath, saveStorageType);

                if (savePath != currentPath)
                {
                    AccessManager.Instance.RegisterDocument(savePath, _currentUser.Name);
                    _currentUser.OwnedDocuments.Add(savePath);
                    _documentManager.CurrentDocument.Title = savePath;
                }

                _documentManager.AddHistoryEntry(savePath, _currentUser.Name, "Document saved");
                Console.WriteLine("Document saved successfully.");
            }
            catch (Exception ex)
            {
                ShowError($"Error saving document: {ex.Message}");
            }
        }

        private void OpenDocument()
        {
            Console.Write("Enter file path: ");
            string path = Console.ReadLine();

            _documentManager.ShowAvailableStorageTypes();
            Console.Write("Enter storage type (local/onedrive): ");
            string storageType = Console.ReadLine().ToLower();

            try
            {
                _documentManager.LoadDocument(path, storageType);

                if (_documentManager.CurrentDocument != null)
                {
                    _documentManager.CurrentDocument.Title = path;
                    _documentManager.CurrentDocument.Attach(_currentUser);

                    if (AccessManager.Instance.GetDocumentOwner(path) == null)
                    {
                        AccessManager.Instance.RegisterDocument(path, _currentUser.Name);
                        _currentUser.OwnedDocuments.Add(path);
                    }

                    _documentManager.AddHistoryEntry(path, _currentUser.Name, "Document opened");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error opening document: {ex.Message}");
            }
        }


        private void InsertText()
        {
            Console.Write("Enter position to insert: ");
            if (int.TryParse(Console.ReadLine(), out int insertPos))
            {
                Console.Write("Enter text to insert: ");
                string text = Console.ReadLine();
                
                _documentManager.CurrentDocument.InsertText(insertPos, text, _currentUser.Name, "Insert operation");
                _documentManager.AddHistoryEntry(_documentManager.CurrentDocument.Title, _currentUser.Name, $"Inserted text at position {insertPos}");
            }
            else
            {
                ShowError("Invalid position");
            }
        }

        private void DeleteText()
        {
            Console.Write("Enter position to delete from: ");
            if (int.TryParse(Console.ReadLine(), out int deletePos))
            {
                Console.Write("Enter number of characters to delete: ");
                if (int.TryParse(Console.ReadLine(), out int deleteLength))
                {
                    _documentManager.CurrentDocument.DeleteText(deletePos, deleteLength, _currentUser.Name);
                    _documentManager.AddHistoryEntry(_documentManager.CurrentDocument.Title, _currentUser.Name, $"Deleted {deleteLength} chars from position {deletePos}");
                }
                else
                {
                    ShowError("Invalid length");
                }
            }
            else
            {
                ShowError("Invalid position");
            }
        }

        private void FormatText()
        {
            Console.Write("Enter start position: ");
            if (int.TryParse(Console.ReadLine(), out int startPos))
            {
                Console.Write("Enter end position: ");
                if (int.TryParse(Console.ReadLine(), out int endPos))
                {
                    Console.WriteLine("Available formats: bold, italic, underline, header1, header2, header3");
                    Console.Write("Enter format: ");
                    string formatType = Console.ReadLine().ToLower();
                    _documentManager.CurrentDocument.ApplyFormat(startPos, endPos, formatType, _currentUser.Name);
                    _documentManager.AddHistoryEntry(_documentManager.CurrentDocument.Title, _currentUser.Name, $"Applied {formatType} format");
                }
                else
                {
                    ShowError("Invalid end position");
                }
            }
            else
            {
                ShowError("Invalid start position");
            }
        }

        private void CopyText()
        {
            Console.Write("Enter start position: ");
            if (int.TryParse(Console.ReadLine(), out int startPos))
            {
                Console.Write("Enter number of characters to copy: ");
                if (int.TryParse(Console.ReadLine(), out int length))
                {
                    _documentManager.CurrentDocument.Copy(startPos, length, _currentUser.Name);
                    Console.WriteLine("Text copied to clipboard");
                }
                else
                {
                    ShowError("Invalid length");
                }
            }
            else
            {
                ShowError("Invalid position");
            }
        }

        private void CutText()
        {
            Console.Write("Enter start position: ");
            if (int.TryParse(Console.ReadLine(), out int startPos))
            {
                Console.Write("Enter number of characters to cut: ");
                if (int.TryParse(Console.ReadLine(), out int length))
                {
                    _documentManager.CurrentDocument.Cut(startPos, length, _currentUser.Name);
                    Console.WriteLine("Text cut to clipboard");
                }
                else
                {
                    ShowError("Invalid length");
                }
            }
            else
            {
                ShowError("Invalid position");
            }
        }

        private void PasteText()
        {
            Console.Write("Enter position to paste: ");
            if (int.TryParse(Console.ReadLine(), out int pos))
            {
                _documentManager.CurrentDocument.Paste(pos, _currentUser.Name);
                Console.WriteLine("Text pasted from clipboard");
            }
            else
            {
                ShowError("Invalid position");
            }
        }

        private void SearchDocument()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            Console.Write("Enter search term: ");
            string term = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(term))
            {
                ShowError("Search term cannot be empty");
                return;
            }

            var results = _documentManager.CurrentDocument.Search(term);

            Console.Clear();
            Console.WriteLine($"Search results for: '{term}'");
            Console.WriteLine("----------------------------------");

            if (results.Count == 0)
            {
                Console.WriteLine("No matches found");
            }
            else
            {
                Console.WriteLine($"Found {results.Count} matches:");
                Console.WriteLine();

                foreach (var result in results)
                {
                    Console.WriteLine($"Position: {result.Position}");
                    Console.WriteLine($"Context: ...{result.Context}...");
                    Console.WriteLine("----------------------------------");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private void CreateDocument()
        {
            if (!_currentUser.CanEdit())
            {
                ShowError("You don't have permission to create documents");
                return;
            }

            _documentManager.ShowAvailableDocumentTypes();
            Console.Write("Enter document type: ");
            string type = Console.ReadLine().ToLower();

            Console.Write("Enter document name/path: ");
            string path = Console.ReadLine();
            AccessManager.Instance.RegisterDocument(path, _currentUser.Name);
            _currentUser.OwnedDocuments.Add(path);

            _documentManager.CreateDocument(type, path);

            if (_documentManager.CurrentDocument != null)
            {
                _documentManager.CurrentDocument.Title = path;
                _documentManager.CurrentDocument.Attach(_currentUser);
                _documentManager.AddHistoryEntry(path, _currentUser.Name, "Document created");
                Console.WriteLine($"Document '{path}' created successfully. You have full access.");
            }
        }

       

        private void ShowHistory()
        {
            Console.Write("Enter document path: ");
            string path = Console.ReadLine();
            _documentManager.ShowDocumentHistory(path);
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

         private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

}
