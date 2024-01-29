using Microsoft.AspNetCore.Mvc;
using Money_Locker_Project.Authenticator;
using MoneyLocker.CommonUtility;
using MoneyLocker.DataAccess;
using MoneyLocker.Model;
using MoneyLocker.Model.User;
using MoneyLocker.Model.Validator;
using System;
using System.Net;
using System.Text.Json;
using static MoneyLocker.CommonUtility.Constants;

namespace Money_Locker_Project.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IAuthenticator Authenticator;
        public readonly IDataAccess DataAccess;

        public UserController(IAuthenticator _authenticator, IDataAccess _dataAccess)
        {
            Authenticator = _authenticator;
            DataAccess = _dataAccess;
        }

        #region User Login
        [Route(API_Route.User + End_Point.Login)]
        [HttpPost]
        public ResponseInfo Login(UserLogin requestInfo)
        {
            ResponseInfo response = new();
            try
            {
                RequestValidator request = new()
                {
                    UserLogin = requestInfo,
                    Selector = Constants.User.Login
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
                    bool isAuthenticated = DataAccess.AuthenticateUser(requestInfo);
                    if (isAuthenticated)
                    {
                        response.Data = "Login Successful";
                        response.IsSuccess = true;
                        response.StatusCode = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.Data = "Login Failed";
                        response.StatusCode = (int)HttpStatusCode.OK;
                    }
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

        #region User SignUp
        [HttpPost]
        [Route(API_Route.User + End_Point.SignUp)]
        public ResponseInfo SignUp(UserSignUp requestInfo)
        {
            ResponseInfo response = new();
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

        #region User Details
        [Route(API_Route.User + End_Point.Details)]
        [HttpGet]
        public ResponseInfo GetUserDetails(long mobileNo, string emailId)
        {
            ResponseInfo response = new();
            try
            {
                UserDetails userDetails = DataAccess.GetUserDetails(mobileNo, emailId);
                if (userDetails != null)
                {
                    response.Message = "User Details Fetched Successfully";
                    response.Data = JsonSerializer.Serialize(userDetails);
                    response.IsSuccess = true;
                    response.StatusCode = (int)HttpStatusCode.OK;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Message = "User Details Not Found";
                    response.StatusCode = (int)HttpStatusCode.NotFound;
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
