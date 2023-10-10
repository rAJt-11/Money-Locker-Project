using MoneyLocker.Data;
using MoneyLocker.Data.Schema;
using MoneyLocker.Model.User;
using System.Linq;

namespace MoneyLocker.DataAccess
{
    public class DataAccess : IDataAccess
    {
        public readonly MoneyLockerDbContext dbContext;

        public DataAccess(MoneyLockerDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public string AddUser(UserSignUp addUser)
        {
            UserInfo userInfo = new()
            {
                FirstName = addUser.FirstName,
                LastName = addUser.LastName,
                Mobile = addUser.Mobile,
                Password = addUser.Password,
                Email = addUser.Email
            };

            try
            {
                dbContext.UserInfo.Add(userInfo);
                dbContext.SaveChangesAsync();
                return "User updated successfully";
            }
            catch
            {
                return "Some error occured while updating user";
            }
        }

        public bool AuthenticateUser(UserLogin userLogin)
        {
            if (!string.IsNullOrEmpty(userLogin.Mobile))
            {
                var user = dbContext.UserInfo.FirstOrDefault(u => u.Mobile == userLogin.Mobile);
                if (user != null && user.Password == userLogin.Password)
                {
                    return true;
                }
            }    
            else
            {
                var user = dbContext.UserInfo.FirstOrDefault(u => u.Email == userLogin.Email);
                if (user != null && user.Password == userLogin.Password)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
