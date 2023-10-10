using MoneyLocker.Model.User;

namespace MoneyLocker.Model.Validator
{
    public class RequestValidator
    {
        public string Selector { get; set; }

        public UserSignUp UserSignUp { get; set; }

        public UserLogin UserLogin { get; set; }
    }
}
