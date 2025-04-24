using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{
    public interface IStorageStrategy
    {
        void Save(string path, string content);
        string Load(string path);
    }
}
