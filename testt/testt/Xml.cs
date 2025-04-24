
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace testt
{
    public class XmlAdapter : IDocumentAdapter
    {
        public string Convert(Document document)
        {
            var xmlDoc = new XmlDocument();
            var root = xmlDoc.CreateElement("Document");
            xmlDoc.AppendChild(root);

            foreach (var component in document.GetContent())
            {
                var element = xmlDoc.CreateElement("Text");
                element.InnerText = component.GetText();
                element.SetAttribute("Format", component.GetFormat());
                element.SetAttribute("Color", component.GetColor().ToString());
                root.AppendChild(element);
            }

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xmlDoc.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public Document ConvertBack(string content)
        {
            var doc = new Document();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(content);

                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    TextComponent textComponent = new PlainText(node.InnerText);
                    var format = node.Attributes["Format"].Value;
                    if (format.Contains("Bold")) textComponent = new BoldText(textComponent);
                    if (format.Contains("Italic")) textComponent = new ItalicText(textComponent);
                    if (format.Contains("Underline")) textComponent = new UnderlineText(textComponent);
                    doc.AddContent(textComponent);
                }
            }
            catch
            {
                doc.AddContent(new PlainText("Invalid XML format"));
            }
            return doc;
        }
    }
}
