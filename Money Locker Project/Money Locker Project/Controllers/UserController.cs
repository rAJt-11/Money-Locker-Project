using Microsoft.AspNetCore.Mvc;
using Money_Locker_Project.Authenticator;
using MoneyLocker.CommonUtility;
using MoneyLocker.DataAccess;
using MoneyLocker.Model;
using System;
using System.Net;

namespace Money_Locker_Project.Controllers
{
    [Route("user")]
    [ApiController]
    public class User : ControllerBase
    {
        public readonly IAuthenticator Authenticator;
        public readonly IDataAccess DataAccess;

        public User(IAuthenticator _Authenticator, IDataAccess _dataAccess)
        {
            Authenticator = _Authenticator;
            DataAccess = _dataAccess;
        }

        #region User Login
        //[Route("/login")]
        //[HttpPost]
        //public IActionResult Login(UserLoginModel loginModel)
        //{
        //    PaymentResponse paymentResponse = new();
        //    ErrorInfo errorResponse = new();
        //    bool isAuthenticated = AuthenticateUser(loginModel);

        //    if (isAuthenticated)
        //    {
        //        // User authenticated successfully

        //        paymentResponse.StatusCode = (int)HttpStatusCode.OK;
        //        paymentResponse.PaymentSuccessMsg = "Login successful";
        //        return Ok(paymentResponse);
        //    }
        //    else
        //    {
        //        // Invalid credentials
        //        errorResponse.ErrorMsg = "Invalid username or password";

        //        return Unauthorized(errorResponse);
        //    }
        //}

        //private bool AuthenticateUser(UserLoginModel loginModel)
        //{
        //    UserInfo userLoginInfo = new();
        //    // Authenticate User
        //    string query = "SELECT CustomerName, Password FROM UserLogin WHERE CustomerName = @UserName ";

        //    using (SqlConnection conn = new("server = LP009311; database = MoneyLocker; Integrated Security = true"))
        //    {
        //        conn.Open();

        //        using (SqlCommand command = new(query, conn))
        //        {
        //            //command.Parameters.AddWithValue("@id", loginModel.CustomerId);
        //            command.Parameters.AddWithValue("@UserName", loginModel.UserName);

        //            SqlDataAdapter dataAdapter = new(command);
        //            DataTable dataTable = new();
        //            dataAdapter.Fill(dataTable);

        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                // Access the PaymentTransactionId from each row
        //                userLoginInfo.Password = (string)row["Password"];
        //                userLoginInfo.User_Name = (string)row["CustomerName"];
        //            }
        //        }
        //    }
        //    return (loginModel.UserName == userLoginInfo.User_Name && loginModel.Password == userLoginInfo.Password);
        //}
        #endregion

        #region User Details
        //[Route("/details")]
        //[HttpGet]
        //public IActionResult GetUserDetails(int customerId)
        //{
        //    // Replace this with your logic to fetch user details from a database or any other source
        //    //User user = GetUser(customerId);
        //    UserInfo userInfo = GetUser(customerId);

        //    if (userInfo == null)
        //    {
        //        return NotFound(); // User not found
        //    }

        //    return Ok(userInfo); // Return user details
        //}

        //private UserInfo GetUser(int customerId)
        //{
        //    UserInfo userDetails = new();
        //    string query = "SELECT CustomerName, MoneyLockerTransactionId FROM MoneyLockerDetails WHERE CustomerId = @id";

        //    using (SqlConnection conn = new("server = LP009311; database = MoneyLocker; Integrated Security = true"))
        //    {
        //        conn.Open();

        //        using (SqlCommand command = new(query, conn))
        //        {
        //            // Set the parameter value for the condition
        //            command.Parameters.AddWithValue("@id", customerId); // Replace 'id' with the actual value you want to use

        //            SqlDataAdapter dataAdapter = new(command);
        //            DataTable dataTable = new DataTable();
        //            dataAdapter.Fill(dataTable);

        //            // Process the retrieved data as needed
        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                // Access the PaymentTransactionId from each row
        //                userDetails.User_Name = (string)row["CustomerName"];
        //                userDetails.Money_Locker_Transaction_Id = (string)row["MoneyLockerTransactionId"];
        //            }
        //        }
        //    }
        //    return userDetails;
        //}
        #endregion

        #region User SignUp
        [HttpPost]
        [Route("/signup")]
        public ResponseModel SignUp(UserSignUp requestInfo)
        {
            ResponseModel response = new();
            try
            {
                RequestValidator request = new()
                {
                    UserSignUp = requestInfo,
                    Selector = Constants.User.SignUp
                };
                ErrorInfo errorInfo = Authenticator.Validate(request);
                if (errorInfo.ErrorList.Count > 0)
                {
                    response.IsSuccess = false;
                    response.ErrorInfo = errorInfo;
                    response.StatusCode = (int)HttpStatusCode.BadRequest; 
                }
                else
                {
                    response.Data = DataAccess.AddUser(requestInfo);
                    response.IsSuccess = true;
                    response.StatusCode = (int)HttpStatusCode.OK;  
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = ex.Message + ex.StackTrace + ex.Source; 
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            return response;
        }
        #endregion

    }
}
