using NLog;
using finalCOMM.MainClasses;

namespace finalCOMM.Actions
{
    public class AccountAction
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly JsonActions _jsonAction;

        public AccountAction(JsonActions jsonAction)
        {
            _jsonAction = jsonAction;
        }

        public void CheckBalance(User user)
        {
            Console.WriteLine("\n$ Current Balances:");
            Console.WriteLine($"   GEL: {user.Balance["GEL"]}");
            Console.WriteLine($"   USD: {user.Balance["USD"]}");
            Console.WriteLine($"   EUR: {user.Balance["EUR"]}");
            Console.WriteLine(new string('-', 40));
        }

        public void Deposit(User user)
        {
            try
            {
                Console.Write("Enter currency (GEL/USD/EUR): ");
                string? currency = Console.ReadLine()?.ToUpper();

                if (currency is not ("GEL" or "USD" or "EUR"))
                {
                    Console.WriteLine("Unsupported currency.");
                    return;
                }

                Console.Write("Enter amount to deposit: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                switch (currency)
                {
                    case "GEL": user.Balance["GEL"] += amount; break;
                    case "USD": user.Balance["USD"] += amount; break;
                    case "EUR": user.Balance["EUR"] += amount; break;
                }

                user.Transactions ??= new List<Transaction>();
                user.Transactions.Add(new Transaction
                {
                    Date = DateTime.Now,
                    Type = "Deposit",
                    Amount = amount,
                    Currency = currency
                });

                _jsonAction.SaveUsers();

                Logger.Info($"Deposited {amount} {currency} to card {user.CardNumber}");
                Console.WriteLine($"Deposited {amount} {currency}");
                Console.WriteLine(new string('-', 40));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Deposit failed.");
                Console.WriteLine("Error during deposit.");
            }
        }

        public void Withdraw(User user)
        {
            try
            {
                Console.Write("Enter currency (GEL/USD/EUR): ");
                string? currency = Console.ReadLine()?.ToUpper();

                if (currency is not ("GEL" or "USD" or "EUR"))
                {
                    Console.WriteLine("Unsupported currency.");
                    return;
                }

                Console.Write("Enter amount to withdraw: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                decimal currentBalance = currency switch
                {
                    "GEL" => user.Balance["GEL"],
                    "USD" => user.Balance["USD"],
                    "EUR" => user.Balance["EUR"],
                    _ => 0
                };

                if (currentBalance < amount)
                {
                    Console.WriteLine("Insufficient funds.");
                    return;
                }

                switch (currency)
                {
                    case "GEL": user.Balance["GEL"] -= amount; break;
                    case "USD": user.Balance["USD"] -= amount; break;
                    case "EUR": user.Balance["EUR"] -= amount; break;
                }

                user.Transactions ??= new List<Transaction>();
                user.Transactions.Add(new Transaction
                {
                    Date = DateTime.Now,
                    Type = "Withdraw",
                    Amount = amount,
                    Currency = currency
                });

                _jsonAction.SaveUsers();

                Logger.Info($"Withdrew {amount} {currency} from card {user.CardNumber}");
                Console.WriteLine($"Withdrawn {amount} {currency}");
                Console.WriteLine(new string('-', 40));
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Withdrawal failed.");
                Console.WriteLine("Error during withdrawal.");
            }
        }

        public void ChangePIN(User user)
        {
            Console.Write("Enter your new 4-digit PIN: ");
            string? newPin = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newPin) || newPin.Length != 4)
            {
                Console.WriteLine("PIN must be exactly 4 digits.");
                return;
            }
            else if (!int.TryParse(newPin, out _))
            {
                Console.WriteLine("PIN must contain only numbers.");
                return;
            }


            user.PIN = newPin;
            _jsonAction.SaveUsers();

            Logger.Info($"User {user.CardNumber} changed PIN successfully.");
            Console.WriteLine("PIN changed successfully.");
            Console.WriteLine(new string('-', 40));
        }

        public void ConvertData(User user)
        {
            try
            {
                Console.WriteLine("Enter source currency (GEL/USD/EUR): ");
                string? fromCurrency = Console.ReadLine()?.ToUpper();

                if (fromCurrency is not ("GEL" or "USD" or "EUR"))
                {
                    Console.WriteLine("Unsupported source currency.");
                    return;
                }

                Console.WriteLine("Enter target currency (GEL/USD/EUR): ");
                string? toCurrency = Console.ReadLine()?.ToUpper();

                if (toCurrency is not ("GEL" or "USD" or "EUR"))
                {
                    Console.WriteLine("Unsupported target currency.");
                    return;
                }

                if (fromCurrency == toCurrency)
                {
                    Console.WriteLine("Source and target currency must be different.");
                    return;
                }

                Console.Write("Enter amount to convert: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                // Check sufficient funds
                if (user.Balance[fromCurrency] < amount)
                {
                    Console.WriteLine("Insufficient funds to convert.");
                    return;
                }

                // Example conversion rates (you can replace with dynamic rates)
                var rates = new Dictionary<string, Dictionary<string, decimal>>
                {
                    { "GEL", new() { { "USD", 0.31m }, { "EUR", 0.28m } } },
                    { "USD", new() { { "GEL", 3.22m }, { "EUR", 0.90m } } },
                    { "EUR", new() { { "GEL", 3.55m }, { "USD", 1.11m } } }
                };

                decimal convertedAmount = amount * rates[fromCurrency][toCurrency];

                // Update balances
                user.Balance[fromCurrency] -= amount;
                user.Balance[toCurrency] += convertedAmount;

                // Add transaction
                user.Transactions ??= new List<Transaction>();
                user.Transactions.Add(new Transaction
                {
                    Date = DateTime.Now,
                    Type = $"Convert {fromCurrency} --> {toCurrency}",
                    Amount = convertedAmount,
                    Currency = toCurrency
                });

                _jsonAction.SaveUsers();

                Logger.Info($"Converted {amount} {fromCurrency} to {convertedAmount:F2} {toCurrency} for card {user.CardNumber}");
                Console.WriteLine($"Successfully converted {amount} {fromCurrency} → {convertedAmount:F2} {toCurrency}");
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Currency conversion not available for selected pair.");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Conversion failed.");
                Console.WriteLine("Error during currency conversion.");
            }
        }

    }
}
