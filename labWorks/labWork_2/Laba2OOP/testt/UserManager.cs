using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace testt
{
    public sealed class UserManager
    {
        private static UserManager _instance;
        private static readonly object _lock = new object();
        private Dictionary<string, User> _users = new Dictionary<string, User>();
        private const string UserDataFile = "users.json";
        private User _currentUser;

        private UserManager()
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (File.Exists(UserDataFile))
            {
                try
                {
                    string json = File.ReadAllText(UserDataFile);
                    _users = JsonSerializer.Deserialize<Dictionary<string, User>>(json)
                             ?? new Dictionary<string, User>();
                }
                catch
                {
                    _users = new Dictionary<string, User>();
                }
            }
        }

        private void SaveUsers()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_users, options);
                File.WriteAllText(UserDataFile, json);
            }
            catch
            {
                
            }
        }

        public static UserManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new UserManager();
                    }
                    return _instance;
                }
            }
        }

        public void AddUser(User user)
        {
            lock (_lock)
            {
                _users[user.Name] = user;
                SaveUsers();
            }
        }

        public User GetUser(string name)
        {
            lock (_lock)
            {
                return _users.TryGetValue(name, out var user) ? user : null;
            }
        }

        public void SetCurrentUser(User user)
        {
            lock (_lock)
            {
                _currentUser = user;
            }
        }

        public User GetCurrentUser()
        {
            lock (_lock)
            {
                return _currentUser;
            }
        }

        public void UpdateUserRole(string name, UserRole newRole)
        {
            lock (_lock)
            {
                if (_users.TryGetValue(name, out var user))
                {
                    user.UpdateRole(newRole);
                    SaveUsers();

                    if (_currentUser != null && _currentUser.Name == name)
                    {
                        _currentUser.UpdateRole(newRole);
                    }
                }
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            lock (_lock)
            {
                return _users.Values.ToList();
            }
        }


    }
}