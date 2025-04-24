
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{

    public class DocumentEditorApp
    {
        private readonly DocumentManager _documentManager = new DocumentManager();
        private User _currentUser;


        public void Run()
        {
            AppSettings.Instance.ApplySettings();
            Console.WriteLine("Welcome to Console Document Editor!");

            // Login
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Select your role (1-3):");
            Console.WriteLine("1. Viewer (read-only)");
            Console.WriteLine("2. Editor (can edit documents)");
            Console.WriteLine("3. Admin (full access)");
            Console.Write("Your choice: ");

            if (int.TryParse(Console.ReadLine(), out int roleChoice) && roleChoice >= 1 && roleChoice <= 3)
            {
                _currentUser = new User(name, (UserRole)(roleChoice - 1));
                Console.WriteLine($"Logged in as {_currentUser.Name} ({_currentUser.Role})");
            }
            else
            {
                Console.WriteLine("Invalid choice, defaulting to Viewer role");
                _currentUser = new User(name, UserRole.Viewer);
            }

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
                        case 4: ToggleViewMode(); break;
                        case 5: exit = true; break;
                        default: ShowError("Invalid choice"); break;
                    }
                }
                else
                {
                    ShowError("Please enter a number");
                }
            }

            Console.WriteLine("Goodbye!");
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Document Operations");
            Console.WriteLine("2. User Settings");
            Console.WriteLine("3. Application Settings");
            Console.WriteLine("4. Toggle View Mode (Current: " +
                             (_documentManager.CurrentDocument?.Mode == Document.DisplayMode.Preview ? "Preview" : "Edit") + ")");
            Console.WriteLine("5. Exit");
            Console.Write("Your choice: ");
        }

        private void ToggleViewMode()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _documentManager.CurrentDocument.Mode = _documentManager.CurrentDocument.Mode == Document.DisplayMode.Preview
                ? Document.DisplayMode.Edit
                : Document.DisplayMode.Preview;

            Console.WriteLine($"Switched to {_documentManager.CurrentDocument.Mode} mode");
            _documentManager.CurrentDocument.Display();
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
                Console.WriteLine("6. Back to Main Menu");
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
                        case 6: back = true; break;
                        default: ShowError("Invalid choice"); break;
                    }
                }
                else
                {
                    ShowError("Please enter a number");
                }
            }
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
            string type = Console.ReadLine();
            _documentManager.CreateDocument(type);
            _documentManager.CurrentDocument?.Attach(_currentUser);
        }

        private void OpenDocument()
        {
            Console.Write("Enter file path: ");
            string path = Console.ReadLine();
            _documentManager.ShowAvailableStorageTypes();
            Console.Write("Enter storage type (local/onedrive): ");
            string storageType = Console.ReadLine();
            _documentManager.LoadDocument(path, storageType);
            _documentManager.CurrentDocument?.Attach(_currentUser);
        }

        private void SaveDocument()
        {
            if (!_currentUser.CanEdit() || _documentManager.CurrentDocument == null)
            {
                ShowError("You don't have permission to save or no document is open");
                return;
            }

            Console.Write("Enter file path: ");
            string savePath = Console.ReadLine();
            _documentManager.ShowAvailableStorageTypes();
            Console.Write("Enter storage type (local/onedrive): ");
            string saveStorageType = Console.ReadLine();
            _documentManager.SaveDocument(savePath, saveStorageType);
        }

        private void ViewDocument()
        {
            if (_documentManager.CurrentDocument == null)
            {
                ShowError("No document is open");
                return;
            }

            _documentManager.CurrentDocument.Display();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void EditDocument()
        {
            if (!_currentUser.CanEdit() || _documentManager.CurrentDocument == null)
            {
                ShowError("You don't have permission to edit or no document is open");
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
                Console.WriteLine("4. Undo");
                Console.WriteLine("5. Redo");
                Console.WriteLine("4. Toggle View Mode (Current: " + (_documentManager.CurrentDocument?.Mode == Document.DisplayMode.Preview ? "Preview" : "Edit") + ")");
                Console.WriteLine("7. Back to Document Menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int editChoice))
                {
                    switch (editChoice)
                    {
                        case 1: InsertText(); break;
                        case 2: DeleteText(); break;
                        case 3: FormatText(); break;
                        case 4: _documentManager.CurrentDocument.Undo(); break;
                        case 5: _documentManager.CurrentDocument.Redo(); break;
                        case 6: ToggleViewMode(); break;
                        case 7: back = true; break;
                        default: ShowError("Invalid choice"); break;
                    }
                }
                else
                {
                    ShowError("Please enter a number");
                }
            }
        }

        private void InsertText()
        {
            Console.Write("Enter position to insert: ");
            if (int.TryParse(Console.ReadLine(), out int insertPos))
            {
                Console.Write("Enter text to insert: ");
                string text = Console.ReadLine();
                _documentManager.CurrentDocument.InsertText(insertPos, text);
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
                    _documentManager.CurrentDocument.DeleteText(deletePos, deleteLength);
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
                    string formatType = Console.ReadLine();
                    _documentManager.CurrentDocument.ApplyFormat(startPos, endPos, formatType);
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

        private void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = AppSettings.Instance.TextColor;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }


        private void UserSettings()
        {
            if (!_currentUser.CanManageUsers())
            {
                Console.WriteLine("You don't have permission to manage users");
                return;
            }

            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("\nUser Settings:");
                Console.WriteLine("1. Change User Role");
                Console.WriteLine("2. Back to Main Menu");
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
                                // In a real app, we'd have a user management system
                                Console.WriteLine($"User {userName} role changed to {(UserRole)(newRole - 1)}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid role selection");
                            }
                            break;
                        case 2:
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
                Console.WriteLine("3. Change Theme");
                Console.WriteLine("4. View Current Settings");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Your choice: ");

                if (int.TryParse(Console.ReadLine(), out int settingsChoice))
                {
                    switch (settingsChoice)
                    {
                        case 1:
                        
                            Console.WriteLine("Available colors:");
                            foreach (var color in Enum.GetValues(typeof(ConsoleColor)))
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
                            foreach (var color in Enum.GetValues(typeof(ConsoleColor)))
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
                        case 3:
                            Console.Write("Enter new theme name: ");
                            AppSettings.Instance.Theme = Console.ReadLine();
                            Console.WriteLine("Theme changed (visual changes may require restart)");
                            break;
                        case 4:
                            Console.Clear();
                            Console.WriteLine("\nCurrent Settings:");
                            Console.WriteLine($"Text Color: {AppSettings.Instance.TextColor}");
                            Console.WriteLine($"Background Color: {AppSettings.Instance.BackgroundColor}");
                            Console.WriteLine($"Theme: {AppSettings.Instance.Theme}");
                            Console.WriteLine($"Font Size: {AppSettings.Instance.FontSize}");
                            break;
                        case 5:
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
            }
        }
    }



}
