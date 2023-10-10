using MoneyLocker.Model;

namespace MoneyLocker.DataAccess
{
    public interface IDataAccess
    {
        public string AddUser(UserSignUp addUser);

        public bool AuthenticateUser(UserLoginModel loginModel);

        public string GetUser(string mobilenumber);
    }
}
