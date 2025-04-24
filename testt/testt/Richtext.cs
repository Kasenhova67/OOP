using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
   
        public class RichTextAdapter : IDocumentAdapter
        {
            public string Convert(Document document)
            {
                var sb = new StringBuilder();
                foreach (var component in document.GetContent())
                {
                    string text = component.GetRawText();
                    string format = component.GetFormat();

                    if (format.Contains("Bold")) text = $"**{text}**";
                    if (format.Contains("Italic")) text = $"*{text}*";
                    if (format.Contains("Underline")) text = $"_{text}_";

                    sb.Append(text);
                }
                return sb.ToString();
            }

            public Document ConvertBack(string content)
            {
                var doc = new Document();
                TextComponent currentComponent = new PlainText(content);

                
                if (content.Contains("**"))
                {
                    currentComponent = new BoldText(currentComponent);
                }
                if (content.Contains("*"))
                {
                    currentComponent = new ItalicText(currentComponent);
                }
                if (content.Contains("_"))
                {
                    currentComponent = new UnderlineText(currentComponent);
                }

                doc.AddContent(currentComponent);
                return doc;
            }
        }
    
}
