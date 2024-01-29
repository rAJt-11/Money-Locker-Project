using System;

namespace MoneyLocker.Model.Payment
{
    public class PaymentInfo
    {
        public long UserId { get; set; }
        public int Amount { get; set; }
        public string Status { get; set; }
        public long TransactionNumber { get; set; }
        public string GatewayResponse { get; set; }
        public string OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
