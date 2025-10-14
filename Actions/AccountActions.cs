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
                Console.WriteLine($"✅ Deposited {amount} {currency}");
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
                    Console.WriteLine("❌ Insufficient funds.");
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
                Console.WriteLine($"✅ Withdrawn {amount} {currency}");
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

            user.PIN = newPin;
            _jsonAction.SaveUsers();

            Logger.Info($"User {user.CardNumber} changed PIN successfully.");
            Console.WriteLine("✅ PIN changed successfully.");
            Console.WriteLine(new string('-', 40));
        }
    }
}
