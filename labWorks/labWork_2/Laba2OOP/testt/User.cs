using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace testt
{
    [Serializable]
    public class User : IObserver
    {
        public string Name { get; set; }
        public UserRole Role { get; set; }
        public List<string> OwnedDocuments { get; set; } = new List<string>();

        public User() { } 

        public User(string name, UserRole role)
        {
            Name = name;
            Role = role;
        }

        public void UpdateRole(UserRole newRole)
        {
            Role = newRole;
            Console.WriteLine($"{Name}'s role changed to {newRole}");
        }

        public void Update(string message)
        {
            Console.WriteLine($"[Notification for {Name}]: {message}");
        }

        public bool CanEdit() => Role == UserRole.Editor || Role == UserRole.Admin;
        public bool CanManageUsers() => Role == UserRole.Admin;


        public bool HasAccess(string documentPath, bool editAccess)
        {
           
            if (Role == UserRole.Admin)
                return true;

            if (Role == UserRole.Viewer && editAccess)
                return false;

            return AccessManager.Instance.HasAccess(documentPath, Name, editAccess);
        }
    }
    public enum UserRole
    {
        Viewer,
        Editor,
        Admin
    }

}
