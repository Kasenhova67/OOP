using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace testt
{
    public sealed class AccessManager
    {

        private static readonly object _lock = new object();


        private static AccessManager _instance;
        public static AccessManager Instance => _instance ??= new AccessManager();
        private Dictionary<string, string> _documentOwners = new Dictionary<string, string>();
        private Dictionary<string, Dictionary<string, AccessLevel>> _accessRights =
            new Dictionary<string, Dictionary<string, AccessLevel>>();

        public enum AccessLevel
        {
            None,
            View,
            Edit
        }

        public void RegisterDocument(string documentPath, string owner)
        {
            _documentOwners[documentPath] = owner;
            GrantAccess(documentPath, owner, owner, AccessLevel.Edit);
        }

        public void GrantAccess(string documentPath, string owner, string user, AccessLevel level)
        {
            if (!_documentOwners.ContainsKey(documentPath)) return;
            if (_documentOwners[documentPath] != owner && owner != "admin") return;

            if (!_accessRights.ContainsKey(documentPath))
            {
                _accessRights[documentPath] = new Dictionary<string, AccessLevel>();
            }

            _accessRights[documentPath][user] = level;
            SaveAccessData();
        }

        public void RevokeAccess(string documentPath, string user)
        {
            if (_accessRights.TryGetValue(documentPath, out var users))
            {
                users.Remove(user);
                SaveAccessData();
            }
        }

        public bool HasAccess(string documentPath, string user, bool editRequired)
        {
            if (_documentOwners.TryGetValue(documentPath, out var owner) && owner == user)
                return true;
            if (_accessRights.TryGetValue(documentPath, out var users) &&
                users.TryGetValue(user, out var level))
            {
                return editRequired ? level >= AccessLevel.Edit : level >= AccessLevel.View;
            }

            return false;
        }

        public string GetDocumentOwner(string documentPath)
        {
            _documentOwners.TryGetValue(documentPath, out var owner);
            return owner;
        }

        private const string AccessDataFile = "access_data.json";

        private void SaveAccessData()
        {
            var data = new AccessData
            {
                DocumentOwners = _documentOwners,
                AccessRights = _accessRights
            };

            string json = JsonSerializer.Serialize(data);
            File.WriteAllText(AccessDataFile, json);
        }

        public void LoadAccessData()
        {
            if (File.Exists(AccessDataFile))
            {
                string json = File.ReadAllText(AccessDataFile);
                var data = JsonSerializer.Deserialize<AccessData>(json);

                _documentOwners = data.DocumentOwners ?? new Dictionary<string, string>();
                _accessRights = data.AccessRights ?? new Dictionary<string, Dictionary<string, AccessLevel>>();
            }
        }

        private class AccessData
        {
            public Dictionary<string, string> DocumentOwners { get; set; }
            public Dictionary<string, Dictionary<string, AccessLevel>> AccessRights { get; set; }
        }



        public Dictionary<string, AccessLevel> GetAccessList(string documentPath)
        {
            if (_accessRights.TryGetValue(documentPath, out var accessList))
            {
                return new Dictionary<string, AccessLevel>(accessList);
            }
            return new Dictionary<string, AccessLevel>();
        }

        [Serializable]
        public class DocumentInfo
        {
            public string Path { get; set; }
            public string Owner { get; set; }
            public DateTime CreatedDate { get; set; }
            public Dictionary<string, AccessLevel> AccessList { get; set; } = new Dictionary<string, AccessLevel>();
        }


    }
}