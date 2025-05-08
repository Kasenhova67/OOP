using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
    public class DocumentAccess
    {
        public string DocumentPath { get; }
        public string Owner { get; }
        public List<string> AllowedUsers { get; } = new List<string>();

        public DocumentAccess(string path, string owner)
        {
            DocumentPath = path;
            Owner = owner;
        }
    }
}
