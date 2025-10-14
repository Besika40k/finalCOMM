using System;
using System.Linq;
using NLog;
using finalCOMM;

namespace finalCOMM.MainClasses
{
    public class Authentication
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly List<User> _users;

        public Authentication(List<User> users)
        {
            _users = users;
        }

        public User? Authenticate()
        {
            try
            {
                Console.Write("Enter card number: ");
                string? cardNumber = Console.ReadLine();

                Console.Write("Enter CVC: ");
                string? cvc = Console.ReadLine();

                Console.Write("Enter expiration date (MM/YY): ");
                string? expDate = Console.ReadLine();

                var user = _users.FirstOrDefault(u =>
                    u.CardNumber == cardNumber &&
                    u.CVC == cvc &&
                    u.ExpirationDate == expDate);

                if (user == null)
                {
                    Console.WriteLine("❌ Invalid card information. Please try again.");
                    Logger.Warn($"Failed card authentication for card {cardNumber}");
                    return null;
                }

                Console.Write("Enter PIN: ");
                string? pin = Console.ReadLine();

                if (user.PIN != pin)
                {
                    Console.WriteLine("❌ Incorrect PIN.");
                    Logger.Warn($"Invalid PIN attempt for card {cardNumber}");
                    return null;
                }

                Logger.Info($"User with card {cardNumber} successfully logged in.");
                Console.WriteLine("✅ Login successful!");
                return user;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during user authentication.");
                Console.WriteLine("An error occurred during authentication.");
                return null;
            }
        }
    }
}
