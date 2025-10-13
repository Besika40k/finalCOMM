using System.Text.Json;
using NLog;
using finalCOMM.MainClasses;

namespace finalCOMM
{
    internal class Program
    {
        // NLog Logger
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // Path to JSON file
        private const string UserDataFile = "Database/users.json";

        // List of users loaded from JSON
        private static List<User> users = new();

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("=== Welcome to the ATM Simulator ===");

                // Load user data
                LoadUsers();

                // Main program loop
                while (true)
                {
                    Console.WriteLine("\nPlease insert your card (enter card number): ");
                    string cardNumber = Console.ReadLine();

                    User currentUser = AuthenticateUser(cardNumber);
                    if (currentUser != null)
                    {
                        Console.WriteLine("Login successful!");
                        logger.Info($"User {cardNumber} logged in.");
                        ShowMenu(currentUser);
                    }
                    else
                    {
                        Console.WriteLine("Invalid card number. Try again.");
                        logger.Warn($"Failed login attempt with card {cardNumber}");
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("User data file not found!");
                logger.Error(ex, "users.json file missing");
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Error parsing user data!");
                logger.Error(ex, "JSON parsing error");
            }
           
        }

        private static void LoadUsers()
        {
            string json = File.ReadAllText(UserDataFile);
            users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private static User AuthenticateUser(string cardNumber)
        {
            // Simple authentication: search by card number
            return users.Find(u => u.CardNumber == cardNumber);
        }

        private static void ShowMenu(User user)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nSelect an operation:");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. Show Last 5 Transactions");
                Console.WriteLine("5. Change PIN");
                Console.WriteLine("6. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("oh this thing ain't working yet");
                        break;
                    case "2":
                        Console.WriteLine("oh this thing ain't working yet");
                        break;
                    case "3":
                        Console.WriteLine("oh this thing ain't working yet");
                        break;
                    case "4":
                        Console.WriteLine("oh this thing ain't working yet");
                        break;
                    case "5":
                        Console.WriteLine("oh this thing ain't working yet");
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }
    }
}
