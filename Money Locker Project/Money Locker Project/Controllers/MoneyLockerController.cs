using Microsoft.AspNetCore.Mvc;
using Money_Locker_Project.Authenticator;
using MoneyLocker.DataAccess;
using MoneyLocker.Model;
using MoneyLocker.Model.Payment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using static MoneyLocker.CommonUtility.Constants;

namespace Money_Locker_Project.Controllers
{
    [ApiController]
    public class MoneyLockerController : ControllerBase
    {
        public readonly IAuthenticator Authenticator;
        public readonly IDataAccess DataAccess;

        public MoneyLockerController(IAuthenticator _authenticator, IDataAccess _dataAccess)
        {
            Authenticator = _authenticator;
            DataAccess = _dataAccess;
        }

        [Route(API_Route.MoneyLocker + End_Point.Transaction)]
        [HttpPost]
        public IActionResult MoneyLockerTransactionProcess(PaymentRequest request)
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
            List<PaymentResponse> successInfo = new();

            // Validating MoneyLocker Transaction Id From Database
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
                CreateRazorpayRequest(moneyLockerTransaction);//Updates Request In DB
                GatewayResponse gatewayResponse = InitiatePaymentProcess(moneyLockerTransaction);
                response.MoneyLockerSuccessMsg = gatewayResponse.MoneyLockerSuccessMsg;
                response.IsSuccess = gatewayResponse.IsSuccess;
                response.StatusCode = gatewayResponse.StatusCode;
            }
            return response;
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
            return errorResponse;
        }

        private void CreateRazorpayRequest(PaymentRequest moneyLockerTransaction)
        {
            PaymentInfo paymentInfo = new()
            {
            UserId = moneyLockerTransaction.UserId,
            Amount = moneyLockerTransaction.Money_Locker_Amount,
            Status = PaymentStatusType.Pre_Init,
            CreatedDate = DateTime.Now
            };
            DataAccess.UpdatePaymentInfo(paymentInfo);
        }

        private GatewayResponse InitiatePaymentProcess(PaymentRequest paymentRequest)
        {
            //MoneyLocker.PaymentGateway.InitiateOrder();
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

                // Get Current Money Locker Amount from DB
                int current_amount = GetMoneyLockerAmount(paymentRequest.Money_Locker_Transaction_Id);
                int newAmount = current_amount + paymentRequest.Money_Locker_Amount;

                // Update Money Locker Amount in DB
                string msg = UpdateMoneyLockerAmount(paymentRequest.Money_Locker_Transaction_Id, newAmount);

                if (msg != null && msg.Contains("SUCCESS", StringComparison.OrdinalIgnoreCase))
                {
                    DateTime currentDateTime = DateTime.Now;
                    string TransactionReferenceNumber = Guid.NewGuid().ToString();
                    gatewayResponse.StatusCode = (int)HttpStatusCode.OK;
                    gatewayResponse.IsSuccess = true;
                    gatewayResponse.MoneyLockerSuccessMsg = $"Congratulations, You deposited Rs {paymentRequest.Money_Locker_Amount} to your Money Locker Account on {currentDateTime}. Your Transaction Ref. No is {TransactionReferenceNumber}.";
                }
                else
                {
                    DateTime currentDateTime = DateTime.Now;
                    string TransactionReferenceNumber = Guid.NewGuid().ToString();
                    gatewayResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    gatewayResponse.IsSuccess = true;
                    gatewayResponse.MoneyLockerSuccessMsg = $"{paymentRequest.Money_Locker_Transaction_Id} is not registered with Money Locker. ";
                }
            }
            return gatewayResponse;
        }

        private int GetMoneyLockerAmount(string Money_Locker_Transaction_Id)
        {
            int amount = 0;
            string query = "SELECT Amount From MoneyLockerAmount WHERE MoneyLockerTransactionId = @Money_Locker_Transaction_Id ";

            using (SqlConnection conn = new("server = LP009311; database = MoneyLocker; Integrated Security = true"))
            {
                conn.Open();

                using (SqlCommand command = new(query, conn))
                {
                    command.Parameters.AddWithValue("@Money_Locker_Transaction_Id", Money_Locker_Transaction_Id);

                    SqlDataAdapter dataAdapter = new(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Access the PaymentTransactionId from each row
                        amount = (int)row["Amount"];
                    }
                }
            }
            return amount;
        }

        private string UpdateMoneyLockerAmount(string Money_Locker_Transaction_Id, int newAmount)
        {
            string updateQuery = "UPDATE MoneyLockerAmount SET Amount = @NewAmount WHERE MoneyLockerTransactionId = @Money_Locker_Transaction_Id";

            using (SqlConnection conn = new SqlConnection("server = LP009311; database = MoneyLocker; Integrated Security = true"))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(updateQuery, conn))
                {
                    command.Parameters.AddWithValue("@NewAmount", newAmount);
                    command.Parameters.AddWithValue("@Money_Locker_Transaction_Id", Money_Locker_Transaction_Id);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the update was successful
                    if (rowsAffected > 0)
                    {
                        return("SUCCESS");
                    }
                    else
                    {
                        return("DECLINED");
                    }
                }
            }
        }

    }

}
