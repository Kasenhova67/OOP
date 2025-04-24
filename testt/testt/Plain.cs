
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{

    public class PlainTextAdapter : IDocumentAdapter
    {
        public string Convert(Document document)
        {
            var sb = new StringBuilder();
            foreach (var component in document.GetContent())
            {
                sb.Append(component.GetText());
            }
            return sb.ToString();
        }

        public Document ConvertBack(string content)
        {
            var doc = new Document();
            doc.AddContent(new PlainText(content));
            return doc;
        }
    }
}
