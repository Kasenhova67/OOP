
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

namespace CourseWork.Serialization
{
    public class JsonSerializer<T> : ISerializer<T>
    {
        public void Serialize(IEnumerable<T> data, string filePath)
        {
            var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public IEnumerable<T> Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        public string SerializeToString(IEnumerable<T> data)
        {
            return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
        }

        public List<T> DeserializeFromString(string content)
        {
            return JsonConvert.DeserializeObject<List<T>>(content) ?? new List<T>();
        }
    }

    public interface ISerializer<T>
    {
        void Serialize(IEnumerable<T> data, string filePath);
        IEnumerable<T> Deserialize(string filePath);
        string SerializeToString(IEnumerable<T> data);
        List<T> DeserializeFromString(string content);
    }
}