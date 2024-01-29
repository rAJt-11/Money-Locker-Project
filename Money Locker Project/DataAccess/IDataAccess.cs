using MoneyLocker.Model.Payment;
using MoneyLocker.Model.User;

namespace MoneyLocker.DataAccess
{
    public interface IDataAccess
    {
        public string AddUser(UserSignUp addUser);

        public bool AuthenticateUser(UserLogin userLogin);

        public UserDetails GetUserDetails(long mobileNo, string emailId);

        public void UpdatePaymentInfo(PaymentInfo paymentInfo);

    }
}
