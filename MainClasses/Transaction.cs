namespace finalCOMM.MainClasses;

public class Transaction
{
    // Date and time of the transaction
    public DateTime Date { get; set; } = DateTime.Now;

    // Type of transaction: "Deposit", "Withdraw", "PIN Change", etc.
    public string Type { get; set; }

    // Amount involved in the transaction
    public decimal Amount { get; set; }

    // Currency of the transaction, e.g., "USD", "EUR", "GEL"
    public string Currency { get; set; }

    // Optional: Constructor for quick creation
    public Transaction() { }

    public Transaction(string type, decimal amount, string currency)
    {
        Type = type;
        Amount = amount;
        Currency = currency;
        Date = DateTime.Now;
    }

    // Optional: Override ToString for easy display
    public override string ToString()
    {
        return $"{Date:G} | {Type} | {Amount} {Currency}";
    }
}