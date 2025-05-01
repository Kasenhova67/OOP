using Xunit;
using testt;
using System;
using System.Collections.Generic;

namespace TestProject
{
    public class AppSettingsTests
    {
        [Fact]
        public void AppSettings_IsSingleton()
        {
            var instance1 = AppSettings.Instance;
            var instance2 = AppSettings.Instance;

            Assert.Same(instance1, instance2);
        }

        
    }

    public class TextComponentTests
    {
        [Fact]
        public void PlainText_GetText_ReturnsCorrectText()
        {
            var text = "Hello";
            var component = new PlainText(text);

            Assert.Equal(text, component.GetText());
            Assert.Equal(text, component.GetRawText());
        }

        [Fact]
        public void BoldText_ModifiesFormat()
        {
            var baseText = new PlainText("Text");
            var boldText = new BoldText(baseText);

            Assert.Contains("Bold", boldText.GetFormat());
            Assert.Equal(ConsoleColor.Yellow, boldText.GetColor());
        }

        [Fact]
        public void HeaderText_AddsLevelToFormat()
        {
            var baseText = new PlainText("Header");
            var header = new HeaderText(baseText, 2);

            Assert.Contains("Header2", header.GetFormat());
            Assert.Equal(ConsoleColor.Green, header.GetColor());
        }
    }

    public class DocumentTests
    {
        [Fact]
        public void InsertText_AddsComponentAtPosition()
        {
            var doc = new Document();
            doc.InsertText(0, "Test");

            Assert.Single(doc.GetContent());
            Assert.Equal("Test", doc.GetContent()[0].GetText());
        }

        [Fact]
        public void DeleteText_RemovesCorrectCharacters()
        {
            var doc = new Document();
            doc.InsertText(0, "Hello World");
            doc.DeleteText(6, 5);

            Assert.Equal("Hello ", doc.GetFullText());
        }

        [Fact]
        public void Undo_ReverseLastAction()
        {
            var doc = new Document();
            doc.InsertText(0, "First");
            doc.InsertText(0, "Second");
            doc.Undo();

            Assert.Equal("First", doc.GetFullText());
        }

       
       
    }

    public class CommandTests
    {
        [Fact]
        public void InsertTextCommand_ExecuteAndUndo()
        {
            var doc = new Document();
            var cmd = new Document.InsertTextCommand(doc, 0, "Test", "");

            cmd.Execute();
            Assert.Equal("Test", doc.GetFullText());

            cmd.Undo();
            Assert.Empty(doc.GetContent());
        }

        
    }

    public class DocumentManagerTests
    {
        [Fact]
        public void CreateDocument_WithValidType()
        {
            var manager = new DocumentManager();
            manager.CreateDocument("plain");

            Assert.NotNull(manager.CurrentDocument);
            Assert.Contains("plain", manager.CurrentDocument.Title.ToLower());
        }

        
    }

    public class AdapterTests
    {
        [Fact]
        public void PlainTextAdapter_ConvertsCorrectly()
        {
            var adapter = new PlainTextAdapter();
            var doc = new Document();
            doc.InsertText(0, "Test");

            string serialized = adapter.Convert(doc);
            Document deserialized = adapter.ConvertBack(serialized);

            Assert.Equal("Test", deserialized.GetFullText());
        }

       
    }

    public class ObserverTests
    {
        [Fact]
        public void Document_NotifiesObservers()
        {
            var doc = new Document();
            var observer = new TestObserver();
            doc.Attach(observer);

            doc.InsertText(0, "Test");

            Assert.Contains("inserted", observer.LastMessage);
        }

        private class TestObserver : IObserver
        {
            public string LastMessage { get; private set; }

            public void Update(string message)
            {
                LastMessage = message;
            }
        }
    }

   
        public class AdvancedTextComponentTests
        {
            [Fact]
            public void ItalicText_ChangesColorToCyan()
            {
                var baseText = new PlainText("Text");
                var italicText = new ItalicText(baseText);

                Assert.Equal(ConsoleColor.Cyan, italicText.GetColor());
                Assert.Contains("Italic", italicText.GetFormat());
            }

            [Fact]
            public void UnderlineText_ChangesColorToDarkRed()
            {
                var baseText = new PlainText("Text");
                var underlined = new UnderlineText(baseText);

                Assert.Equal(ConsoleColor.DarkRed, underlined.GetColor());
                Assert.Contains("Underline", underlined.GetFormat());
            }

            [Fact]
            public void NestedDecorators_MaintainAllFormats()
            {
                var text = new PlainText("Hello");
                var boldItalic = new ItalicText(new BoldText(text));

                Assert.Contains("Bold", boldItalic.GetFormat());
                Assert.Contains("Italic", boldItalic.GetFormat());
            }
        }

        public class DocumentEdgeCaseTests
        {
            [Fact]
            public void InsertText_AtNegativePosition_ThrowsException()
            {
                var doc = new Document();
                Assert.Throws<ArgumentOutOfRangeException>(() => doc.InsertText(-1, "Test"));
            }

            [Fact]
            public void DeleteText_BeyondDocumentLength_DeletesToEnd()
            {
                var doc = new Document();
                doc.InsertText(0, "Short");
                doc.DeleteText(2, 100);

                Assert.Equal("Sh", doc.GetFullText());
            }

            [Fact]
            public void ApplyFormat_ToEmptyDocument_DoesNothing()
            {
                var doc = new Document();
                doc.ApplyFormat(0, 5, "bold"); // Не должно вызывать исключений

                Assert.Empty(doc.GetContent());
            }

            [Fact]
            public void Undo_WithEmptyStack_DoesNothing()
            {
                var doc = new Document();
                doc.Undo(); // Не должно вызывать исключений

                Assert.Empty(doc.GetContent());
            }
        }

      

        public class DocumentManagerAdvancedTests
        {
            [Fact]
            public void CreateDocument_WithInvalidType_ShowsError()
            {
                var manager = new DocumentManager();
                manager.CreateDocument("invalid_type");

                Assert.Null(manager.CurrentDocument);
            }

            [Fact]
            public void LoadDocument_WithInvalidPath_ShowsError()
            {
                var manager = new DocumentManager();
                manager.LoadDocument("nonexistent_file.txt");

                Assert.Null(manager.CurrentDocument);
            }

            [Fact]
            public void SaveDocument_WithoutCurrentDocument_ShowsError()
            {
                var manager = new DocumentManager();
                manager.SaveDocument("test.txt"); // Не должно вызывать исключений

                Assert.Null(manager.CurrentDocument);
            }
        }

      

        public class StorageStrategyTests
        {
            [Fact]
            public void LocalFileStorage_SaveAndLoad_Roundtrip()
            {
                var storage = new LocalFileStorage();
                string testPath = Path.GetTempFileName();
                string testContent = "Test content";

                storage.Save(testPath, testContent);
                string loaded = storage.Load(testPath);

                Assert.Equal(testContent, loaded);
                File.Delete(testPath);
            }

            [Fact]
            public void OneDriveStorage_WithInvalidPath_ReturnsEmptyString()
            {
                var storage = new OneDriveLocalStorage();
                string result = storage.Load("invalid_path.txt");

                Assert.Equal(string.Empty, result);
            }
        }

        public class DisplayModeTests
        {
            [Fact]
            public void Document_DisplayPreviewMode_ShowsFormattedText()
            {
                var doc = new Document();
                doc.InsertText(0, "Test");
                doc.ApplyFormat(0, 0, "bold");

                doc.Mode = Document.DisplayMode.Preview;
                doc.Display(); // Не должно вызывать исключений
            }

            [Fact]
            public void Document_DisplayEditMode_ShowsMarkup()
            {
                var doc = new Document();
                doc.InsertText(0, "Test");
                doc.ApplyFormat(0, 0, "bold");

                doc.Mode = Document.DisplayMode.Edit;
                doc.Display(); // Не должно вызывать исключений
            }
        }

        
    
}