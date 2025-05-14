using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3OOp.DataAccess
{
    public static class JsonFileHelper
    {
        public static T ReadFromJsonFile<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default;
        }

        public static void WriteToJsonFile<T>(string filePath, T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
