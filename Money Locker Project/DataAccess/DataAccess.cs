using MoneyLocker.Data;
using MoneyLocker.Data.Schema;
using MoneyLocker.Model.Payment;
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
            if (userLogin.Mobile > 0)
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

        public UserDetails GetUserDetails(long mobileNo, string emailId)
        {
            UserDetails user = new();
            if (mobileNo > 0)
            {
                var userInfo = dbContext.UserInfo.FirstOrDefault(u => u.Mobile == mobileNo);
                user.FirstName = userInfo.FirstName;    
                user.LastName = userInfo.LastName;
                user.Email = userInfo.Email;     
            }
            else
            {
                var userInfo = dbContext.UserInfo.FirstOrDefault(u => u.Email == emailId);
                user.FirstName = userInfo.FirstName;
                user.LastName = userInfo.LastName;
                user.Mobile= userInfo.Mobile;
            }

            return user;
        }

        public void UpdatePaymentInfo(PaymentInfo paymentInfo)
        {
            Payment payment = new()
            {
                UserId = paymentInfo.UserId,
                CreatedDate = paymentInfo.CreatedDate,
                Amount = paymentInfo.Amount,
                Status = paymentInfo.Status
            };

            try
            {
                dbContext.Payment.Add(payment);
                dbContext.SaveChangesAsync(); 
            }
            catch
            {
                
            }
        }
    }
}
