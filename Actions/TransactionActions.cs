using System;
using finalCOMM.MainClasses;

namespace finalCOMM.Actions
{
    public class TransactionActions
    {
        public void ShowLastTransactions(User user, int count = 5)
        {
            Console.WriteLine($"\nLast {count} transactions:");

            var recent = user.Transactions
                .TakeLast(count)
                .ToList();

            if (!recent.Any())
            {
                Console.WriteLine("No transactions found.");
                return;
            }

            foreach (var t in recent)
                Console.WriteLine($"• {t}");
        }
    }
}