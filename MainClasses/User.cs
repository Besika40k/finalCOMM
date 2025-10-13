namespace finalCOMM.MainClasses;

public class User
{
    // Card information
    public string CardNumber { get; set; }        // 16-digit card number
    public string CVC { get; set; }               // 3-digit card security code
    public string ExpirationDate { get; set; }    // Format: MM/YY

    // Account security
    public string PIN { get; set; }               // 4-digit PIN code

    // Account balances in different currencies
    public Dictionary<string, decimal> Balance { get; set; } = new Dictionary<string, decimal>();

    // Transaction history
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    // Default constructor
    public User() { }

    // Optional: Constructor with initial data
    public User(string cardNumber, string cvc, string expirationDate, string pin)
    {
        CardNumber = cardNumber;
        CVC = cvc;
        ExpirationDate = expirationDate;
        PIN = pin;
        Balance = new Dictionary<string, decimal>
        {
            { "GEL", 0m },
            { "USD", 0m },
            { "EUR", 0m }
        };
        Transactions = new List<Transaction>();
    }
}