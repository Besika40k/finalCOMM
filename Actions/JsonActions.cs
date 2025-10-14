
using System.Text.Json;
using NLog;
using finalCOMM.MainClasses;

namespace finalCOMM.Actions
{
    public class JsonActions
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _filePath;
        public List<User> Users { get; private set; }

        public JsonActions(string filePath)
        {
            string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..");
            _filePath = Path.Combine(projectRoot, "Database", "users.json");
            _filePath = Path.GetFullPath(_filePath);
            Users = LoadUsers();
        }

        public List<User> LoadUsers()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
                    File.WriteAllText(_filePath, "[]");
                    Logger.Warn("users.json not found. New file created.");
                    return new List<User>();
                }

                string json = File.ReadAllText(_filePath);
                var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
                Logger.Info("Users loaded successfully.");
                return users;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to load users.json");
                Console.WriteLine("Error loading user data.");
                return new List<User>();
            }
        }

        public void SaveUsers()
        {
            try
            {
                string json = JsonSerializer.Serialize(Users, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
                Logger.Info("User data saved successfully.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to save users.json");
                Console.WriteLine("Error saving user data.");
            }
        }
    }
}