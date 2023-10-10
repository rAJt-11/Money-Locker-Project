namespace MoneyLocker.Model.Payment
{
    public class PaymentRequest
    {
        public string Payment_Transaction_Id { get; set; }
        public string Money_Locker_Transaction_Id { get; set; }
        public int Payment_Amount { get; set; }
        public int Money_Locker_Amount { get; set; }
        public bool MoneyLocker_IsOpted { get; set; }
        public string Money_Locker_OrderId { get; set; }
    }

    //public class TransactionRequest
    //{
    //    public MerchantTransaction MerchantTransaction { get; set; }
    //    public MoneyLockerTransaction MoneyLockerTransaction { get; set; }
    //}

    //public class MerchantTransaction
    //{
    //    public string Payment_Transaction_Id { get; set; }
    //    public int Payment_Amount { get; set; }
    //}

    //public class MoneyLockerTransaction
    //{
    //    public string Money_Locker_Transaction_Id { get; set; }
    //    public int Money_Locker_Amount { get; set; }
    //}
}
