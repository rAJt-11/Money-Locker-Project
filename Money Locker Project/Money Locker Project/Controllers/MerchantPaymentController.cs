using Microsoft.AspNetCore.Mvc;
using MoneyLocker.Model;
using MoneyLocker.Model.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;

namespace Money_Locker_Project.Controllers
{
    [ApiController]
    [Route("api/merchant/payments")]
    public class MerchantPaymentController : ControllerBase
    {
     
        [HttpPost]
        public IActionResult MerchantPaymentProcess(PaymentRequest request)
        {
            try
            {
                // Validate the payment request
                ErrorInfo errorResponse = ValidateRequest(request);
                if (errorResponse.ErrorList.Count > 0)
                {
                    return BadRequest(errorResponse);
                }

                // Process the payment
                PaymentResponse paymentResponse = PaymentTransactionProcess(request);

                // Return the payment response
                return Ok(paymentResponse);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Payment processing error: {ex.Message}");

                // Return a generic error message
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while processing the payment.");
            }
        }

        private PaymentResponse PaymentTransactionProcess(PaymentRequest request)
        {
            // Payment Gateway Logic and Processing
            PaymentResponse response = new();
            StringBuilder stringBuilder = new();
            GatewayResponse gatewayResponse = new();

            // In Case Of MoneyLocker Is Not Opted by Customer    
            if (request.MoneyLocker_IsOpted == false)
            {
                // Validating Payment Transaction Id From Database
                PaymentTransactionId isExist = ValidatePaymentTransactionId(request.Payment_Transaction_Id);
                if (isExist == null)
                {
                    var errors = new ErrorDetailInfo
                    {
                        Type = "Payment_Transaction_Id_Not_Exists"
                    };
                    response.ErrorList.Add(errors);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    PaymentRequest merchantTransaction = new();
                    merchantTransaction.Payment_Amount = request.Payment_Amount;
                    merchantTransaction.Payment_Transaction_Id = request.Payment_Transaction_Id;
                    gatewayResponse = InitiatePaymentProcess(merchantTransaction);
                    response.IsSuccess = gatewayResponse.IsSuccess;
                    response.StatusCode = gatewayResponse.StatusCode;
                    response.PaymentSuccessMsg = gatewayResponse.PaymentSuccessMsg;
                }
            }

            // In Case Of MoneyLocker Is Opted by Customer
            else if (request.MoneyLocker_IsOpted == true)
            {
                // Validating MoneyLocker Transaction Id From Database
                PaymentTransactionId isExist = ValidatePaymentTransactionId(request.Payment_Transaction_Id);
                if (isExist == null)
                {
                    var errors = new ErrorDetailInfo
                    {
                        Type = "Payment_Transaction_Id_Not_Exists"
                    };        
                    response.ErrorList.Add(errors);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                }
                else
                {
                    PaymentRequest merchantTransaction = new();
                    merchantTransaction.Payment_Amount = request.Payment_Amount;
                    merchantTransaction.Payment_Transaction_Id = request.Payment_Transaction_Id;
                    gatewayResponse = InitiatePaymentProcess(merchantTransaction);   
                    //stringBuilder.AppendLine($"MerchantPaymentSuccessMsg : {response.Msg}");
                }
                response.PaymentSuccessMsg = gatewayResponse.PaymentSuccessMsg;
                PaymentTransactionId isValid = ValidateMoneyLockerTransactionId(request.Money_Locker_Transaction_Id);
                if (isValid == null)
                {
                    var errors = new ErrorDetailInfo
                    {
                        Type = "Money_Locker_Transaction_Id_Not_Exists"
                    };
                    response.ErrorList.Add(errors);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                }
                else
                {
                    PaymentRequest moneyLockerTransaction = new();
                    moneyLockerTransaction.Money_Locker_Amount = request.Money_Locker_Amount;
                    moneyLockerTransaction.Money_Locker_Transaction_Id = request.Money_Locker_Transaction_Id;
                    gatewayResponse = InitiatePaymentProcess(moneyLockerTransaction);
                    //response.Msg = gatewayResponse.Msg;
                    //stringBuilder.AppendLine($"MoneyLockerSuccessMsg : {response.Msg}");
                }
                //string result = stringBuilder.ToString();
                //result.Replace("\n", "").Replace("\r", "");
                //response.PaymentSuccessMsg = gatewayResponse.PaymentSuccessMsg;
                response.MoneyLockerSuccessMsg = gatewayResponse.MoneyLockerSuccessMsg;
                response.IsSuccess = gatewayResponse.IsSuccess;
                response.StatusCode = gatewayResponse.StatusCode;
            }
            return response;
        }

        private PaymentTransactionId ValidatePaymentTransactionId(string Payment_Transaction_Id)
        {
            PaymentTransactionId isExist = new();

            SqlConnection conn = new("server = LP009311; database = MoneyLocker; Integrated Security = true");
            SqlDataAdapter data = new("Select PaymentTransactionId from MerchantDetails", conn);
            DataTable dt = new();
            data.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                List<PaymentTransactionId> Existing_Transaction_Ids = dt.AsEnumerable()
                    .Select(row => new PaymentTransactionId
                    {
                        PaymentTransactionIds = row["PaymentTransactionId"].ToString()
                    }).ToList();

                isExist = Existing_Transaction_Ids.FirstOrDefault(item => item.PaymentTransactionIds == Payment_Transaction_Id);  
            }
            return isExist;
        }

        private PaymentTransactionId ValidateMoneyLockerTransactionId(string Money_Locker_Transaction_Id)
        {
            PaymentTransactionId isValid = new();

            SqlConnection conn = new("server = LP009311; database = MoneyLocker; Integrated Security = true");
            SqlDataAdapter data = new("Select MoneyLockerTransactionId from MoneyLockerDetails", conn);
            DataTable dt = new();
            data.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                List<PaymentTransactionId> Existing_Money_Locker_Transaction_Ids = dt.AsEnumerable()
                    .Select(row => new PaymentTransactionId
                    {
                        PaymentTransactionIds = row["MoneyLockerTransactionId"].ToString()
                    }).ToList();

                isValid = Existing_Money_Locker_Transaction_Ids.FirstOrDefault(item => item.PaymentTransactionIds == Money_Locker_Transaction_Id);
            }
            return isValid;
        }

        private ErrorInfo ValidateRequest(PaymentRequest request)
        {
            ErrorInfo errorResponse = new();

            if (string.IsNullOrEmpty(request.Payment_Transaction_Id))
            {
                var errors = new ErrorDetailInfo();
                errors.Type = "Missing_Payment_Transaction_Id";
                errorResponse.ErrorList.Add(errors);
            }
            if (request.Payment_Amount <= 0)
            {
                var errors = new ErrorDetailInfo
                {
                    Type = "Invalid_Payment_Amount"
                };
                errorResponse.ErrorList.Add(errors);
            }
            if (request.MoneyLocker_IsOpted == true)
            {
                if (string.IsNullOrEmpty(request.Money_Locker_Transaction_Id))
                {
                    var errors = new ErrorDetailInfo
                    {
                        Type = "Missing_Money_Locker_Transaction_Id"
                    };
                    errorResponse.ErrorList.Add(errors);
                }
                if (request.Money_Locker_Amount <= 0)
                {
                    var errors = new ErrorDetailInfo
                    {
                        Type = "Invalid_MoneyLocker_Amount"
                    };
                    errorResponse.ErrorList.Add(errors);
                }
            }
            return errorResponse;
        }

        private GatewayResponse InitiatePaymentProcess(PaymentRequest paymentRequest)
        {
            string paymentGatewayUrl = "https://payment-gateway-url.com/api/initiate-payment";

            // Convert paymentRequest to JSON
            string jsonPayload = JsonConvert.SerializeObject(paymentRequest);

            // Send HTTP POST request to the payment gateway
            GatewayResponse jsonResponse = PostData(paymentGatewayUrl, jsonPayload);

            // Handle the response from the payment gateway
            if (jsonResponse.StatusCode == (int)HttpStatusCode.OK)
            {
                return jsonResponse;
            }
            else
            {
                // Handle the error response from the payment gateway
                throw new Exception("Failed to initiate payment. Status code: " + (int)HttpStatusCode.BadGateway);
            }
        }

        private GatewayResponse PostData(string paymentGatewayUrl, string jsonPayload)
        {
            GatewayResponse gatewayResponse = new();
            if (paymentGatewayUrl != null && paymentGatewayUrl.Contains("initiate-payment", StringComparison.OrdinalIgnoreCase))
            {
                PaymentRequest paymentRequest = JsonConvert.DeserializeObject<PaymentRequest>(jsonPayload);
                List<GatewayResponse>gatewaySuccessInfo = new();
                if (!String.IsNullOrEmpty(paymentRequest.Payment_Transaction_Id))
                {
                    DateTime currentDateTime = DateTime.Now;
                    string TransactionReferenceNumber = Guid.NewGuid().ToString();
                    gatewayResponse.StatusCode = (int)HttpStatusCode.OK;
                    gatewayResponse.IsSuccess = true;
                    gatewayResponse.PaymentSuccessMsg = $"Dear User, Your transaction of Rs {paymentRequest.Payment_Amount} on {currentDateTime} is successfull. Your Transaction Ref. No is {TransactionReferenceNumber}.";
                    //gatewaySuccessInfo.Add(gatewayResponse);
                }
                if (!String.IsNullOrEmpty(paymentRequest.Money_Locker_Transaction_Id))
                {
                    DateTime currentDateTime = DateTime.Now;
                    string TransactionReferenceNumber = Guid.NewGuid().ToString();
                    gatewayResponse.StatusCode = (int)HttpStatusCode.OK;
                    gatewayResponse.IsSuccess = true;
                    gatewayResponse.MoneyLockerSuccessMsg = $"Congratulations, You deposited Rs {paymentRequest.Money_Locker_Amount} to your Money Locker Account on {currentDateTime}. Your Transaction Ref. No is {TransactionReferenceNumber}.";
                    //gatewaySuccessInfo.Add(gatewayResponse);
                }
            }
            return gatewayResponse;
        }
    
    }
}
