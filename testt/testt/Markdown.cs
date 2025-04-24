using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testt
{
    public class MarkdownAdapter : IDocumentAdapter
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
            var lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                var headerMatch = Regex.Match(line, @"^(#+)\s(.+)$");
                if (headerMatch.Success)
                {
                    int level = headerMatch.Groups[1].Value.Length;
                    string text = headerMatch.Groups[2].Value;
                    var component = new HeaderText(new PlainText(text), level);
                    doc.AddContent(component);
                }
                else
                {
                    doc.AddContent(new PlainText(line + Environment.NewLine));
                }
            }

            return doc;
        }
    }
}
