using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{

    public class PlainTextFactory : IDocumentFactory
    {
        public Document CreateDocument()
        {
            return new Document();
        }
    }

    public class MarkdownFactory : IDocumentFactory
    {
        public Document CreateDocument()
        {
            var doc = new Document();
            
            return doc;
        }
    }

    public class RichTextFactory : IDocumentFactory
    {
        public Document CreateDocument()
        {
            var doc = new Document();
           
            return doc;
        }
    }
}
