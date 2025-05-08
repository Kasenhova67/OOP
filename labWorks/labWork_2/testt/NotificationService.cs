using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using testt;

public class NotificationService
{

    private static readonly object _lock = new();
    private const string UserNotificationsDir = "UserNotifications";
    private const string TempNotificationsFile = "temp_notifications.json";

    private Dictionary<string, List<DocumentChange>> _pendingChanges = new();

    public void TrackChange(string documentPath, string changeDescription, string modifiedBy)
    {
        lock (_lock)
        {
            if (!_pendingChanges.ContainsKey(documentPath))
            {
                _pendingChanges[documentPath] = new List<DocumentChange>();
            }

            _pendingChanges[documentPath].Add(new DocumentChange
            {
                Description = changeDescription,
                Timestamp = DateTime.Now,
                ModifiedBy = modifiedBy
            });
        }
    }

    public void CommitChanges(string documentPath)
    {
        lock (_lock)
        {
            if (!_pendingChanges.TryGetValue(documentPath, out var changes) || !changes.Any())
                return;

            var subscribers = UserManager.Instance.GetDocumentSubscribers(documentPath);

            foreach (var username in subscribers)
            {
                AddNotificationsToUser(username, documentPath, changes);
            }

            _pendingChanges.Remove(documentPath);
        }
    }

    private void AddNotificationsToUser(string username, string documentPath, List<DocumentChange> changes)
    {
        var user = UserManager.Instance.GetUser(username);
        if (user == null) return;

        foreach (var change in changes)
        {
            user.GetNotifications().Add(new UserNotification(
                $"{change.ModifiedBy} made change: {change.Description}",
                documentPath
            ));
        }

        user.SaveNotifications();
    }

    private class DocumentChange
    {
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string ModifiedBy { get; set; }
    }

    
    public void SendNotificationToSubscribers(string documentPath, string message, string modifiedBy)
    {
        lock (_lock)
        {
            var subscribers = UserManager.Instance.GetDocumentSubscribers(documentPath);
            if (!subscribers.Any()) return;

            foreach (var username in subscribers)
            {
                AddNotificationToUser(username, new UserNotification(
                    $"{modifiedBy} modified document: {message}",
                    documentPath
                ));
            }
        }
    }

    private void AddNotificationToUser(string username, UserNotification notification)
    {
        string filePath = Path.Combine("UserNotifications", $"{username}.json");
        var notifications = LoadUserNotifications(filePath);
        notifications.Add(notification);

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(filePath, JsonSerializer.Serialize(notifications, options));
    }




    private Dictionary<string, List<DocumentChange>> _tempChanges = new();

    static NotificationService()
    {
        if (!Directory.Exists(UserNotificationsDir))
            Directory.CreateDirectory(UserNotificationsDir);
    }


    private List<UserNotification> LoadUserNotifications(string filePath)
    {
        if (!File.Exists(filePath))
            return new List<UserNotification>();

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<UserNotification>>(File.ReadAllText(filePath), options)
                   ?? new List<UserNotification>();
        }
        catch
        {
            return new List<UserNotification>();
        }
    }


    private void SaveTempNotifications()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(TempNotificationsFile, JsonSerializer.Serialize(_tempChanges, options));
    }


}