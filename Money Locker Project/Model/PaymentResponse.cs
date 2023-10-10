using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MoneyLocker.Model
{
    public class PaymentResponse
    {
        [JsonProperty("paymentSuccessMsg")]
        public string PaymentSuccessMsg { get; set; }
        [JsonProperty("moneyLockerSuccessMsg")]
        public string MoneyLockerSuccessMsg { get; set; }
        [JsonProperty("success")]
        public bool IsSuccess { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonIgnore]
        public List<ErrorDetailInfo> ErrorList { get; set; } = new List<ErrorDetailInfo>();
    }

    //public class Message
    //{
    //    [JsonProperty("paymentSuccessMsg")]
    //    public string PaymentSuccessMsg { get; set; }
    //    [JsonProperty("moneyLockerSuccessMsg")]
    //    public string MoneyLockerSuccessMsg { get; set; }
    //}

    public class GatewayResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string PaymentSuccessMsg { get; set; }
        public string MoneyLockerSuccessMsg { get; set; }



        public string Payment_Ref_Id { get; set; }
        public DateTime Payment_Date { get; set; }
        public string Payment_Transaction_Id { get; set; }
        public string Money_Locker_Transaction_Id { get; set; }
        public int Payment_Amount { get; set; }
        public int Money_Locker_Amount { get; set; }
        public bool MoneyLocker_IsOpted { get; set; }
    }

}
