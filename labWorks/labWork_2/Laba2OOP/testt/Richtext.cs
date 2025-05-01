using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace testt
{
    public class RichTextAdapter : IDocumentAdapter
    {
        public string Convert(Document document)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<richtext>");

            foreach (var component in document.GetContent())
            {
                string text = System.Net.WebUtility.HtmlEncode(component.GetRawText());
                string format = component.GetFormat();

                sb.Append("<text");
                if (format.Contains("Bold")) sb.Append(" bold=\"true\"");
                if (format.Contains("Italic")) sb.Append(" italic=\"true\"");
                if (format.Contains("Underline")) sb.Append(" underline=\"true\"");
                if (format.Contains("Header1")) sb.Append(" header=\"1\"");
                if (format.Contains("Header2")) sb.Append(" header=\"2\"");
                if (format.Contains("Header3")) sb.Append(" header=\"3\"");

                sb.Append(">");
                sb.Append(text);
                sb.AppendLine("</text>");
            }

            sb.AppendLine("</richtext>");
            return sb.ToString();
        }

        public Document ConvertBack(string content)
        {
            var doc = new Document();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);

                foreach (XmlNode node in xmlDoc.DocumentElement.SelectNodes("text"))
                {
                    string text = System.Net.WebUtility.HtmlDecode(node.InnerText);
                    TextComponent component = new PlainText(text);

                    if (node.Attributes["bold"]?.Value == "true") component = new BoldText(component);
                    if (node.Attributes["italic"]?.Value == "true") component = new ItalicText(component);
                    if (node.Attributes["underline"]?.Value == "true") component = new UnderlineText(component);

                    string header = node.Attributes["header"]?.Value;
                    if (!string.IsNullOrEmpty(header))
                    {
                        component = new HeaderText(component, int.Parse(header));
                    }

                    doc.AddContent(component);
                }
            }
            catch
            {
                doc.AddContent(new PlainText("Invalid RichText format"));
            }
            return doc;
        }
    }
}
