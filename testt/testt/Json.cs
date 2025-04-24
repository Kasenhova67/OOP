
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace testt
{
    public class JsonAdapter : IDocumentAdapter
    {
        public string Convert(Document document)
        {
            var contentList = document.GetContent().Select(c => new
            {
                Text = c.GetText(),
                Color = c.GetColor().ToString(),
                Format = c.GetFormat()
            }).ToList();

            return JsonSerializer.Serialize(contentList);
        }

        public Document ConvertBack(string content)
        {
            var doc = new Document();
            try
            {
                var jsonElements = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);
                foreach (var element in jsonElements)
                {
                    TextComponent textComponent = new PlainText(element["Text"]);
                    if (element["Format"].Contains("Bold")) textComponent = new BoldText(textComponent);
                    if (element["Format"].Contains("Italic")) textComponent = new ItalicText(textComponent);
                    if (element["Format"].Contains("Underline")) textComponent = new UnderlineText(textComponent);
                    doc.AddContent(textComponent);
                }
            }
            catch
            {
                doc.AddContent(new PlainText("Invalid JSON format"));
            }
            return doc;
        }
    }


}
