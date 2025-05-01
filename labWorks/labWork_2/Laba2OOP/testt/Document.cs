
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace testt
{

    public class Document : ISubject
    {
        public DisplayMode Mode { get; set; } = DisplayMode.Edit;

        public void Display()
        {
            Console.WriteLine($"\n--- {Title} ---");
            foreach (var component in _content)
            {
                Console.Write(component.GetText());
            }
            Console.WriteLine("\n--- End of Document ---");
        }

        public void DisplayPreview()
        {
            Console.WriteLine($"\n--- {Title} [Preview] ---");
            foreach (var component in _content)
            {
                Console.ForegroundColor = component.GetColor();
                Console.Write(component.GetText());
                Console.ResetColor();
            }
            Console.WriteLine("\n--- End of Document ---");
        }
        public enum DisplayMode
        {
            Preview,
            Edit
        }

        public void DisplayEdit() => DispEdit();
       
      
        public void DispEdit()
        {
            Console.WriteLine($"\n--- {Title} [Edit] ---");
            foreach (var component in _content)
            {
                Console.Write(GetEditModeText(component));
            }
            Console.WriteLine("\n--- End of Document ---");
        }
      

        private void DisplayPreviewMode()
        {
            foreach (var component in _content)
            {
                
                Console.ForegroundColor = component.GetColor();
                Console.Write(component.GetText());
                Console.ForegroundColor = AppSettings.Instance.TextColor;
            }
        }

        private void DisplayEditMode()
        {
            foreach (var component in _content)
            {
                Console.Write(GetEditModeText(component));
            }
        }

        private string GetEditModeText(TextComponent component)
        {
            string text = component.GetRawText();
            string format = component.GetFormat();

            if (format.Contains("Header"))
            {
                int level = int.Parse(format.Split(new[] { "Header" }, StringSplitOptions.None)[1]);
                return new string('#', level) + " " + text;
            }

            if (format.Contains("Bold")) text = $"**{text}**";
            if (format.Contains("Italic")) text = $"*{text}*";
            if (format.Contains("Underline")) text = $"_{text}_";

            return text;
        }
       
        

        private readonly List<TextComponent> _content = new List<TextComponent>();
        private readonly List<IObserver> _observers = new List<IObserver>();
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        public string Title { get; set; } = "Untitled Document";


        public List<SearchResult> Search(string searchTerm)
        {
            var results = new List<SearchResult>();
            string fullText = GetFullText();

            if (string.IsNullOrEmpty(fullText) || string.IsNullOrEmpty(searchTerm))
                return results;

            int index = 0;
            while ((index = fullText.IndexOf(searchTerm, index, StringComparison.OrdinalIgnoreCase)) >= 0)
            {
              
                int startContext = Math.Max(0, index - 20);
                int endContext = Math.Min(fullText.Length, index + searchTerm.Length + 20);
                string context = fullText.Substring(startContext, endContext - startContext);

                context = context.Replace("\r\n", " ").Replace("\n", " ");

                results.Add(new SearchResult
                {
                    Position = index,
                    Context = context
                });

                index += searchTerm.Length;
            }

            return results;
        }

        public class SearchResult
        {
            public int Position { get; set; }
            public string Context { get; set; }
        }


        public void AddContent(TextComponent component)
        {
            _content.Add(component);
        }

        public void InsertText(int position, string text, string format = "Plain")
        {
            var command = new InsertTextCommand(this, position, text, format);
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            Notify($"Text inserted at position {position}");
        }

        public void DeleteText(int position, int length)
        {
            
            int startIndex = position;
            int endIndex = position + length - 1; 

            var command = new DeleteRangeCommand(this, startIndex, endIndex);
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            Notify($"Deleted {length} characters from position {position}");
        }

       
        public void Undo()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                command.Undo();
                _redoStack.Push(command);
                Notify("Undo performed");
            }
        }

        public void Redo()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                command.Execute();
                _undoStack.Push(command);
                Notify("Redo performed");
            }
        }

        public List<TextComponent> GetContent() => _content;

        public string GetFullText()
        {
            var sb = new StringBuilder();
            foreach (var component in _content)
            {
                sb.Append(component.GetText());
            }
            return sb.ToString();
        }

       
        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        private string _clipboard = string.Empty;

        public void Copy(int start, int length)
        {
            string text = GetTextRange(start, length);
            if (!string.IsNullOrEmpty(text))
            {
                _clipboard = text;
                Notify($"Copied {length} characters to clipboard");
            }
        }

        public void Cut(int start, int length)
        {
            string text = GetTextRange(start, length);
            if (!string.IsNullOrEmpty(text))
            {
                _clipboard = text;
                DeleteText(start, length);
                Notify($"Cut {length} characters to clipboard");
            }
        }

        public void Paste(int position)
        {
            if (!string.IsNullOrEmpty(_clipboard))
            {
                InsertText(position, _clipboard);
                Notify($"Pasted {_clipboard.Length} characters from clipboard");
            }
        }

        private string GetTextRange(int start, int length)
        {
            string fullText = GetFullText();
            if (start < 0 || start >= fullText.Length) return string.Empty;

            length = Math.Min(length, fullText.Length - start);
            return fullText.Substring(start, length);
        }
        public class InsertTextCommand : ICommand
        {
            private readonly Document _document;
            private readonly int _position;
            private readonly string _text;
            private readonly string _format;

            public InsertTextCommand(Document document, int position, string text, string format)
            {
                _document = document;
                _position = position;
                _text = text;
                _format = format;
            }

            public void Execute()
            {
                TextComponent component = new PlainText(_text);

                if (!string.IsNullOrEmpty(_format))
                {
                    if (_format.Contains("bold")) component = new BoldText(component);
                    if (_format.Contains("italic")) component = new ItalicText(component);
                    if (_format.Contains("underline")) component = new UnderlineText(component);
                    if (_format.Contains("header1")) component = new HeaderText(component, 1);
                    if (_format.Contains("header2")) component = new HeaderText(component, 2);
                    if (_format.Contains("header3")) component = new HeaderText(component, 3);
                }

                if (_position >= _document._content.Count)
                {
                    _document._content.Add(component);
                }
                else
                {
                    _document._content.Insert(_position, component);
                }
            }

            public void Undo()
            {
                if (_position < _document._content.Count)
                {
                    _document._content.RemoveAt(_position);
                }
            }
        }


        public void DeleteRange(int startIndex, int endIndex)
        {
            var command = new DeleteRangeCommand(this, startIndex, endIndex);
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            Notify($"Deleted characters from {startIndex} to {endIndex}");
        }


        public class DeleteRangeCommand : ICommand
        {
            private readonly Document _document;
            private readonly int _startIndex;
            private readonly int _endIndex;
            private List<TextModification> _modifications;

            public DeleteRangeCommand(Document document, int startIndex, int endIndex)
            {
                _document = document;
                _startIndex = Math.Max(0, startIndex);
                _endIndex = endIndex;
                _modifications = new List<TextModification>();
            }

            public void Execute()
            {
                int currentGlobalIndex = 0;
                int remainingStart = _startIndex;
                int remainingEnd = _endIndex;

                for (int i = 0; i < _document.GetContent().Count && remainingStart <= remainingEnd; i++)
                {
                    var component = _document.GetContent()[i];
                    string text = component.GetRawText();
                    int componentLength = text.Length;

                    int startInComponent = Math.Max(0, remainingStart - currentGlobalIndex);
                    int endInComponent = Math.Min(componentLength - 1, remainingEnd - currentGlobalIndex);

                    if (startInComponent <= endInComponent)
                    {
                        
                        string newText = text.Remove(startInComponent, endInComponent - startInComponent + 1);
                        var newComponent = CreateComponentWithSameFormat(component, newText);

                        _modifications.Add(new TextModification
                        {
                            Index = i,
                            OriginalComponent = component,
                            ModifiedComponent = newComponent,
                            StartInComponent = startInComponent,
                            EndInComponent = endInComponent
                        });

                        _document.GetContent()[i] = newComponent;
                        remainingEnd -= (endInComponent - startInComponent + 1);
                    }

                    currentGlobalIndex += componentLength;
                }
            }

            public void Undo()
            {
                
                for (int i = _modifications.Count - 1; i >= 0; i--)
                {
                    var mod = _modifications[i];
                    _document.GetContent()[mod.Index] = mod.OriginalComponent;
                }
            }

            private TextComponent CreateComponentWithSameFormat(TextComponent original, string newText)
            {
                TextComponent component = new PlainText(newText);

                if (original.GetFormat().Contains("Bold")) component = new BoldText(component);
                if (original.GetFormat().Contains("Italic")) component = new ItalicText(component);
                if (original.GetFormat().Contains("Underline")) component = new UnderlineText(component);

                return component;
            }

            private class TextModification
            {
                public int Index { get; set; }
                public TextComponent OriginalComponent { get; set; }
                public TextComponent ModifiedComponent { get; set; }
                public int StartInComponent { get; set; }
                public int EndInComponent { get; set; }
            }
        }

        private readonly DocumentManager _documentManager;
        private void ToggleViewMode()
        {
            if (_documentManager.CurrentDocument == null)
            {
                Console.WriteLine("No document is open!");
                return;
            }

            _documentManager.CurrentDocument.Mode = _documentManager.CurrentDocument.Mode == Document.DisplayMode.Preview
                ? Document.DisplayMode.Edit
                : Document.DisplayMode.Preview;

            Console.WriteLine($"Switched to {_documentManager.CurrentDocument.Mode} mode");
            _documentManager.CurrentDocument.Display();
        }
        public void ApplyFormat(int start, int end, string formatType)
        {
            var command = new FormatTextCommand(this, start, end, formatType);
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
            Notify($"Applied {formatType} format to text from {start} to {end}");
        }


        public class FormatTextCommand : ICommand
        {
            private readonly Document _document;
            private readonly int _start;
            private readonly int _end;
            private readonly string _formatType;
            private List<TextComponent> _originalComponents;

            public FormatTextCommand(Document document, int start, int end, string formatType)
            {
                _document = document;
                _start = start;
                _end = end;
                _formatType = formatType;
                _originalComponents = new List<TextComponent>();
            }

            public void Execute()
            {
                _originalComponents.Clear();
                int safeEnd = Math.Min(_end, _document._content.Count);

                for (int i = _start; i < safeEnd; i++)
                {
                    _originalComponents.Add(_document._content[i]);
                    TextComponent newComponent = _document._content[i];

                    switch (_formatType.ToLower())
                    {
                        case "bold":
                            newComponent = new BoldText(newComponent);
                            break;
                        case "italic":
                            newComponent = new ItalicText(newComponent);
                            break;
                        case "underline":
                            newComponent = new UnderlineText(newComponent);
                            break;
                        case "header1":
                            newComponent = new HeaderText(newComponent, 1);
                            break;
                        case "header2":
                            newComponent = new HeaderText(newComponent, 2);
                            break;
                        case "header3":
                            newComponent = new HeaderText(newComponent, 3);
                            break;
                    }

                    _document._content[i] = newComponent;
                }
            }

            public void Undo()
            {
                if (_originalComponents != null)
                {
                    int safeEnd = Math.Min(_start + _originalComponents.Count, _document._content.Count);
                    for (int i = 0; i < _originalComponents.Count; i++)
                    {
                        if (_start + i < _document._content.Count)
                        {
                            _document._content[_start + i] = _originalComponents[i];
                        }
                    }
                }
            }
        }
        
    }
}
