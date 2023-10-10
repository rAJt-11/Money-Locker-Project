using MoneyLocker.Model.User;

namespace MoneyLocker.DataAccess
{
    public interface IDataAccess
    {
        public string AddUser(UserSignUp addUser);

        public bool AuthenticateUser(UserLogin userLogin);

    }
}
