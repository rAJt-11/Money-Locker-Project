namespace MoneyLocker.CommonUtility
{
    public class Constants
    {
        public struct User
        {
            public const string Login = "LOGIN";
            public const string Details = "DETAILS";
            public const string SignUp = "SIGNUP";
        }

        public struct RegX
        {
            public const string Mobile = "^[6-9][0-9]{9}$";
            public const string Email = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            public const string Password = "^[a-zA-Z]{1}(?=.*\\d)(?=.*[@#$%&*_])(?=.*[a-zA-Z]).{7,}$";
        }

        /**
        ^: Start of the line.
        [a-zA-Z]{1}: Matches exactly one alphabetic character (either uppercase or lowercase).
        (?=.*\d): Positive lookahead assertion to ensure there is at least one digit in the password.
        (?=.*[@#$%&*_]): Positive lookahead assertion to ensure there is at least one special character among @, #, $, %, &, *, or _ in the password.
        (?=.*[a-zA-Z]): Positive lookahead assertion to ensure there is at least one alphabetic character in the password.
        .{7,}: Matches any character (including digits, special characters, and alphabetic characters) at least 7 or more times.
        $: End of the line.

        This regular expression enforces the following password requirements:

        Starts with an alphabetic character (either uppercase or lowercase).
        Contains at least one digit.
        Contains at least one of the specified special characters (@, #, $, %, &, *, or _).
        Contains at least one alphabetic character.
        Has a minimum length of 8 characters.
        **/

        public struct API_Route
        {
            public const string User = "/api/User"; 
            public const string MerchatPayment = "/api/MerchatPayment";
            public const string MoneyLocker = "/api/MoneyLocker";
        }

        public struct End_Point
        {
            //User
            public const string Login = "/login";
            public const string SignUp = "/signup";
            public const string Details = "/userDetails";

            //MoneyLocker
            public const string Transaction = "/transaction";

            //MerchatPayment
            public const string Payments = "/payments";
        }

        public struct PaymentStatusType
        {
            public const string Auth = "AUTH";
            public const string Init = "INIT";
            public const string Pre_Init = "PRE_INIT";
        }

    }
}
