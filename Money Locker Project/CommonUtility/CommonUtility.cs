using System.Text;
using System;

namespace MoneyLocker.CommonUtility
{
    public class CommonUtility
    {
        private static readonly Random random = new();

        public static string GenerateTransactionId()
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder transactionId = new();

            for (int i = 0; i < 11; i++)
            {
                int randomIndex = random.Next(0, characters.Length);
                transactionId.Append(characters[randomIndex]);
            }

            return transactionId.ToString();
        }
    }
}
