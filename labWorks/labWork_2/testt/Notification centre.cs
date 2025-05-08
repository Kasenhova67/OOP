using System;
using System.Text.Json.Serialization;

namespace testt
{
    public class UserNotification
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("documentPath")]
        public string DocumentPath { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }

        public UserNotification(string message, string documentPath)
        {
            Message = message;
            DocumentPath = documentPath;
            Timestamp = DateTime.Now;
            IsRead = false;
        }
    }
}