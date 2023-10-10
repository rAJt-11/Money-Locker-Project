using MoneyLocker.Data;
using MoneyLocker.Data.Schema;
using MoneyLocker.Model;

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

        public bool AuthenticateUser(UserLoginModel loginModel)
        {
            //var user = dbContext.UserInfo.FirstOrDefault(u => u.Phone == loginModel.Phone);

            //if (user != null && user.Password == loginModel.Password)
            //{
            //    return true;
            //}

            return false;
        }

        public string GetUser(string mobilenumber)
        {
            // Use LINQ to query the database for the user
            //var user = dbContext.UserInfo.FirstOrDefault(u => u.Phone == mobilenumber);

            //User data = JsonSerializer.Deserialize<User>(user);

            // Return the user entity if found, or null if not found

            return string.Empty;
        }
    }
}
