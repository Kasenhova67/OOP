
using System.Text.Json;

namespace testt
{
    public class DocumentHistory
    {

        private List<DocumentChange> _changes = new List<DocumentChange>();
        private readonly string _historyFilePath;

        public DocumentHistory(string documentPath)
        {
            _historyFilePath = Path.ChangeExtension(documentPath, ".history");
            LoadHistory();
        }

        public void AddChange(string user, string description)
        {
            var change = new DocumentChange
            {
                User = user,
                Description = description,
                Timestamp = DateTime.Now
            };
            _changes.Add(change);
            SaveHistory();
        }

        private void LoadHistory()
        {
            if (File.Exists(_historyFilePath))
            {
                try
                {
                    var json = File.ReadAllText(_historyFilePath);
                    _changes = JsonSerializer.Deserialize<List<DocumentChange>>(json) ?? new List<DocumentChange>();
                }
                catch
                {
                    _changes = new List<DocumentChange>();
                }
            }
        }

        private void SaveHistory()
        {
            try
            {
                var json = JsonSerializer.Serialize(_changes);
                File.WriteAllText(_historyFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving history: {ex.Message}");
            }
        }

        public void DisplayHistory()
        {
            Console.WriteLine("\nDocument History:");
            foreach (var change in _changes.OrderByDescending(c => c.Timestamp))
            {
                Console.WriteLine($"[{change.Timestamp}] {change.User}: {change.Description}");
            }
        }
    }

    public class DocumentChange
    {
        public string User { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
