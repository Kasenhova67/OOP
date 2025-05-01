using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace testt
{
    public class MarkdownAdapter : IDocumentAdapter
    {
        public string Convert(Document document)
        {
            var sb = new StringBuilder();
            foreach (var component in document.GetContent())
            {
                string text = component.GetRawText();
                string format = component.GetFormat();

                if (format.Contains("Header1")) text = $"# {text}";
                else if (format.Contains("Header2")) text = $"## {text}";
                else if (format.Contains("Header3")) text = $"### {text}";
                if (format.Contains("Bold")) text = $"**{text}**";
                if (format.Contains("Italic")) text = $"*{text}*";
                if (format.Contains("Underline")) text = $"__{text}__";

                sb.Append(text);
            }
            return sb.ToString();
        }

        public Document ConvertBack(string content)
        {
            var doc = new Document();
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                TextComponent component = new PlainText(line + Environment.NewLine);

               
                if (line.StartsWith("### "))
                {
                    component = new HeaderText(new PlainText(line.Substring(4)), 3);
                }
                else if (line.StartsWith("## "))
                {
                    component = new HeaderText(new PlainText(line.Substring(3)), 2);
                }
                else if (line.StartsWith("# "))
                {
                    component = new HeaderText(new PlainText(line.Substring(2)), 1);
                }
                else if (line.Contains("**") || line.Contains("__"))
                {
                    component = new BoldText(component);
                }
                else if (line.Contains("*") || line.Contains("_"))
                {
                    component = new ItalicText(component);
                }

                doc.AddContent(component);
            }

            return doc;
        }
    }

   
}
