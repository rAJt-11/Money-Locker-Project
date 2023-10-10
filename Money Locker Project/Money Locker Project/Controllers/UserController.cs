using Microsoft.AspNetCore.Mvc;
using Money_Locker_Project.Authenticator;
using MoneyLocker.CommonUtility;
using MoneyLocker.DataAccess;
using MoneyLocker.Model;
using MoneyLocker.Model.User;
using MoneyLocker.Model.Validator;
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
        [Route("/login")]
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
        [Route("/signup")]
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

    }
}
