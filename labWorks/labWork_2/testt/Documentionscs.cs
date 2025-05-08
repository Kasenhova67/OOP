using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text.Json;

namespace testt
{
    public class Doc
    {

            public string Title { get; set; }
            private readonly Stack<DocumentState> _undoStack = new Stack<DocumentState>();
            private readonly List<TextComponent> _content = new List<TextComponent>();
            private readonly Stack<DocumentState> _redoStack = new Stack<DocumentState>();
            private string _clipboard = string.Empty;

/*
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
            }*/

           /* public List<SearchResult> Search(string term)
            {
                var results = new List<SearchResult>();
                int position = 0;

                foreach (var component in _content)
                {
                    string text = component.GetRawText();
                    int index = 0;

                    while ((index = text.IndexOf(term, index, StringComparison.OrdinalIgnoreCase)) >= 0)
                    {
                        string context = GetContext(text, index, term.Length);
                        results.Add(new SearchResult(position + index, context));
                        index += term.Length;
                    }

                    position += text.Length;
                }

                return results;
            }

            public void InsertText(int position, string text)
            {
                if (position < 0 || position > GetDocumentLength())
                    throw new ArgumentOutOfRangeException(nameof(position));

                SaveState();
            TextComponent component = new PlainText(text);
            _content.Insert(position, component);
            }

            public void DeleteText(int position, int length)
            {
                if (position < 0 || position >= _content.Count)
                    throw new ArgumentOutOfRangeException(nameof(position));

                SaveState();
                _content.RemoveRange(position, Math.Min(length, _content.Count - position));
            }
*/
           /* public void ApplyFormat(int start, int end, string formatType)
            {
                if (start < 0 || end >= _content.Count || start > end)
                    throw new ArgumentOutOfRangeException();

                SaveState();
                for (int i = start; i <= end; i++)
                {
                    _content[i].SetFormat(formatType);
                }
            }
*//*
            public void Copy(int start, int length)
            {
                _clipboard = string.Join("", _content
                    .Skip(start)
                    .Take(length)
                    .Select(c => c.GetRawText()));
            }
*/
          /*  public void Cut(int start, int length)
            {
                SaveState();
                Copy(start, length);
                DeleteText(start, length);
            }

            public void Paste(int position)
            {
                if (!string.IsNullOrEmpty(_clipboard))
                {
                    SaveState();
                    InsertText(position, _clipboard);
                }
            }

            public void Undo()
            {
                if (_undoStack.Count > 0)
                {
                    var state = _undoStack.Pop();
                    _redoStack.Push(new DocumentState(_content));
                    _content.Clear();
                    _content.AddRange(state.Components);
                }
            }*/

          /*  public void Redo()
            {
                if (_redoStack.Count > 0)
                {
                    var state = _redoStack.Pop();
                    _undoStack.Push(new DocumentState(_content));
                    _content.Clear();
                    _content.AddRange(state.Components);
                }
            }
*/
            private int GetDocumentLength() => _content.Sum(c => c.GetRawText().Length);

            private string GetContext(string text, int index, int length)
            {
                int start = Math.Max(0, index - 20);
                int end = Math.Min(text.Length, index + length + 20);
                return text.Substring(start, end - start);
            }

            private void SaveState()
            {
                _undoStack.Push(new DocumentState(_content));
                _redoStack.Clear();
            }

            public class SearchResult
            {
                public int Position { get; }
                public string Context { get; }

                public SearchResult(int position, string context)
                {
                    Position = position;
                    Context = context;
                }
            }

            private class DocumentState
            {
                public List<TextComponent> Components { get; }

                public DocumentState(List<TextComponent> components)
                {
                    Components = new List<TextComponent>(components.Select(c => c.Clone()));
                }
            }
        }
    }