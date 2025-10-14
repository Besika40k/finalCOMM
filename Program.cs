using System;
using NLog;
using finalCOMM.MainClasses;
using finalCOMM.Actions;

namespace finalCOMM
{
    internal class Program
    {
        // NLog Logger
        /*
         * private static readonly Logger logger = LogManager.GetCurrentClassLogger();
         */
        private static readonly Logger logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {

                logger.Info("ATM Simulator started");

                Console.WriteLine("=== Welcome to the ATM Simulator ===");

                // Path to JSON file
                string jsonPath = "Database/users.json";

                // Initialize JSON helper and load users
                var jsonActions = new JsonActions(jsonPath);

                // Initialize account actions
                var accountAction = new AccountAction(jsonActions);
                
                // Starting the logging
                
                while (true)
                {
                    Console.WriteLine("\nPlease insert your card (enter card number): ");
                    string? cardNumber = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(cardNumber))
                    {
                        Console.WriteLine("Card number cannot be empty.");
                        continue;
                    }
                    
                    Console.WriteLine("\nPlease insert your CVC (last 3 digits): ");
                    
                    string? cvc = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(cvc))
                    {
                        Console.WriteLine("CVC number cannot be empty.");
                        continue;
                    }
                    
                    Console.WriteLine("\nPlease insert your cards Expiration Date (MM/DD): ");
                    string? expDate = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(expDate))
                    {
                        Console.WriteLine("Card Expiration Date cannot be empty.");
                        continue;
                    }
                    
                    
                    User? currentUser = AuthenticateUser(cardNumber, cvc, expDate, jsonActions);

                    if (currentUser != null)
                    {
                        Console.WriteLine("✅ Login successful!");
                        logger.Info($"User {cardNumber} logged in.");
                        ShowMenu(currentUser, accountAction);
                    }
                    else
                    {
                        Console.WriteLine("❌ Invalid card information. Try again.");
                        logger.Warn($"Failed login attempt with card {cardNumber}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error occurred. Exiting.");
                logger.Error(ex, "Fatal error in Main");
            }
        }

        private static User? AuthenticateUser(string cardNumber, string cvc, string expDate,JsonActions jsonActions)
        {
            // Look for a user with the given card number
            return jsonActions.Users.Find(u => u.CardNumber == cardNumber && u.CVC == cvc &&  u.ExpirationDate == expDate);
        }

        private static void ShowMenu(User user, AccountAction accountAction)
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
                Console.WriteLine("6. Logout");

                Console.Write("Enter choice: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        accountAction.CheckBalance(user);
                        break;
                    case "2":
                        accountAction.Deposit(user);
                        break;
                    case "3":
                        accountAction.Withdraw(user);
                        break;
                    case "4":
                        ShowTransactions(user);
                        break;
                    case "5":
                        accountAction.ChangePIN(user);
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
            }
        }

        private static void ShowTransactions(User user)
        {
            Console.WriteLine("\n📄 Last 5 Transactions:");
            if (user.Transactions == null || user.Transactions.Count == 0)
            {
                Console.WriteLine("No transactions available.");
                return;
            }

            // Show last 5 transactions (or fewer if less than 5)
            var last5 = user.Transactions.Count <= 5
                ? user.Transactions
                : user.Transactions.GetRange(user.Transactions.Count - 5, 5);

            foreach (var t in last5)
            {
                Console.WriteLine($"{t.Date:yyyy-MM-dd HH:mm} | {t.Type} | {t.Amount} {t.Currency}");
            }
        }
    }
}
