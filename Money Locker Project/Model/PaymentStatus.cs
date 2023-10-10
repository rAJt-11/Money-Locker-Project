using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyLocker.Model
{
    public class PaymentStatus
    {
        public string Payment_Ref_Id { get; set; }
        public DateTime Payment_Date { get; set; }
        public string Status { get; set; }
        public string Payment_Transaction_Id { get; set; }
        public string Money_Locker_Transaction_Id { get; set; }
        public int Payment_Amount { get; set; }
        public int Money_Locker_Amount { get; set; }
        public bool MoneyLocker_IsOpted { get; set; }
        public int CustomerId { get; set; }
        public int MerchantId { get; set; }
    }
}
