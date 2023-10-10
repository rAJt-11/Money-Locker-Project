using MoneyLocker.CommonUtility;
using MoneyLocker.Model;
using System;
using System.Text.RegularExpressions;

namespace Money_Locker_Project.Authenticator
{
    public class Validation : IAuthenticator
    {
        ErrorInfo errorInfo = new();

        public ErrorInfo Validate(RequestValidator request)
        {
            
            if (request.Selector.Equals(Constants.User.SignUp, StringComparison.OrdinalIgnoreCase))
            {
                errorInfo = SignUpValidator(request.UserSignUp);
                if (errorInfo.ErrorList.Count > 0)
                {
                    errorInfo.ErrorMsg = "Request cannot be proceeded";
                    errorInfo.Type = "Validation error";
                }
            }

            return errorInfo;
        }

        private ErrorInfo SignUpValidator(UserSignUp request)
        {
            ErrorDetailInfo errors = new();

            if (string.IsNullOrEmpty(request.FirstName))
            {
                errors.Type = "Missing Parameter";
                errors.ErrorMsg = "User first name is missing in request payload";
                errorInfo.ErrorList.Add(errors);
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                errors.Type = "Missing Parameter";
                errors.ErrorMsg = "User last name is missing in request payload";
                errorInfo.ErrorList.Add(errors);
            }

            if (string.IsNullOrEmpty(request.Mobile))
            {
                errors.Type = "Missing Parameter";
                errors.ErrorMsg = "User mobile number is missing in request payload";
                errorInfo.ErrorList.Add(errors);
            }
            else if (!Regex.IsMatch(request.Mobile, Constants.RegX.Mobile))
            {
                errors.Type = "Invalid Parameter";
                errors.ErrorMsg = "User mobile number is invalid in request payload";
                errorInfo.ErrorList.Add(errors);
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                errors.Type = "Missing Parameter";
                errors.ErrorMsg = "User email is missing in request payload";
                errorInfo.ErrorList.Add(errors);
            }
            else if (!Regex.IsMatch(request.Email, Constants.RegX.Email))
            {
                errors.Type = "Invalid Parameter";
                errors.ErrorMsg = "User email is invalid in request payload";
                errorInfo.ErrorList.Add(errors);
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                errors.Type = "Missing Parameter";
                errors.ErrorMsg = "User password is missing in request payload";
                errorInfo.ErrorList.Add(errors);
            }
            else if (!Regex.IsMatch(request.Password, Constants.RegX.Password))
            {
                errors.Type = "Invalid Parameter";
                errors.ErrorMsg = "User password is invalid in request payload";
                errorInfo.ErrorList.Add(errors);
            }

            return errorInfo;
        }
    }
}
