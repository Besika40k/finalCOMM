using System;
using System.Globalization;
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
                // CARD NUMBER
                string cardNumber;
                while (true)
                {
                    Console.Write("Enter card number (16 digits): ");
                    cardNumber = Console.ReadLine() ?? "";
                    if (cardNumber.Length == 16 && long.TryParse(cardNumber, out _))
                        break;
                    Console.WriteLine("Card number must be exactly 16 digits and numeric.");
                }

                // CVC
                string cvc;
                while (true)
                {
                    Console.Write("Enter CVC (3 digits): ");
                    cvc = Console.ReadLine() ?? "";
                    if (cvc.Length == 3 && int.TryParse(cvc, out _))
                        break;
                    Console.WriteLine("CVC must be exactly 3 digits and numeric.");
                }

                // Expiration Date
                string expDate;
                while (true)
                {
                    Console.Write("Enter expiration date (MM/YY): ");
                    expDate = Console.ReadLine() ?? "";
                    if (DateTime.TryParseExact(expDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                        break;
                    Console.WriteLine("Expiration date must be in MM/YY format.");
                }

                // Find user
                var user = _users.FirstOrDefault(u =>
                    u.CardNumber == cardNumber &&
                    u.CVC == cvc &&
                    u.ExpirationDate == expDate);

                if (user == null)
                {
                    Console.WriteLine("Invalid card information. Authentication failed.");
                    Logger.Warn($"Failed card authentication for card {cardNumber}");
                    return null;
                }

                // PIN (user can retry if incorrect)
                while (true)
                {
                    Console.Write("Enter PIN (4 digits): ");
                    string pin = Console.ReadLine() ?? "";
                    if (pin.Length != 4 || !int.TryParse(pin, out _))
                    {
                        Console.WriteLine("PIN must be exactly 4 digits and numeric.");
                        continue;
                    }

                    if (user.PIN == pin)
                    {
                        Logger.Info($"User with card {cardNumber} successfully logged in.");
                        Console.WriteLine("Login successful!");
                        return user;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect PIN. Please try again.");
                        Logger.Warn($"Invalid PIN attempt for card {cardNumber}");
                    }
                }
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
