using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
    public interface IDocumentAdapter
    {
        string Convert(Document document);
        Document ConvertBack(string content);
    }
}
