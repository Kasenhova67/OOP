
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testt
{

    public class User : IObserver
    {
        public string Name { get; }
        public UserRole Role { get; private set; }

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
    }


    public enum UserRole
    {
        Viewer,
        Editor,
        Admin
    }

}
