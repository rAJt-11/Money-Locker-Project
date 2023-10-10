using MoneyLocker.Model.Payment;
using Newtonsoft.Json;
using Razorpay.Api;
using System.Text;

namespace MoneyLocker.PaymentGateway
{
    public class RazorpayGateway
    {

        private static readonly Random random = new();


        public static PaymentRequest InitiateOrder(PaymentRequest moneyLockerTransaction)
        {
            moneyLockerTransaction.Money_Locker_Transaction_Id = GenerateTransactionId();
            Dictionary<string, object> input = new()
            {
                { "amount", Convert.ToDecimal(moneyLockerTransaction.Money_Locker_Amount)*100 },
                { "currency", "INR" },
                { "receipt", moneyLockerTransaction.Money_Locker_Transaction_Id }
            };

            string key = "rzp_test_dlNiakIk2dB8d9";
            string secret = "OvGyN9CCSmVydU5Twkx9hEwj";

            RazorpayClient client = new(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);

            moneyLockerTransaction.Money_Locker_OrderId = order["id"].ToString();
            return moneyLockerTransaction;


        }

        public static string CaptureOrder(int Money_Locker_Amount, string Money_Locker_OrderId)
        {
            string key = "rzp_test_dlNiakIk2dB8d9";
            string secret = "OvGyN9CCSmVydU5Twkx9hEwj";

            RazorpayClient client = new(key, secret);
            Payment payment = client.Payment.Fetch(Money_Locker_OrderId);

            Dictionary<string, object> input = new()
            {
                { "amount", Convert.ToDecimal(Money_Locker_Amount)*100 },
                { "currency", "INR" },
            };
            Payment paymentCaptured = payment.Capture(input);

            string paymentCapturedJson = JsonConvert.SerializeObject(paymentCaptured);

            return paymentCapturedJson;
        }


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
