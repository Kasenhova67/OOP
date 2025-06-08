using System.Collections.Generic;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CourseWork.Serialization
{
    public class XmlSerializer<T> : ISerializer<T>
    {
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<T>));

        public void Serialize(IEnumerable<T> data, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    _serializer.Serialize(writer, new List<T>(data));
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new SerializationException($"XML serialization failed for type {typeof(T).Name}", ex);
            }
        }

        public IEnumerable<T> Deserialize(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<T>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    return (IEnumerable<T>)_serializer.Deserialize(reader) ?? new List<T>();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException($"XML deserialization failed for file {filePath}", ex);
            }
        }

        public string SerializeToString(IEnumerable<T> data)
        {
            using (var writer = new StringWriter())
            {
                _serializer.Serialize(writer, new List<T>(data));
                return writer.ToString();
            }
        }

        public List<T> DeserializeFromString(string content)
        {
            using (var reader = new StringReader(content))
            {
                return (List<T>)_serializer.Deserialize(reader) ?? new List<T>();
            }
        }
    }
}