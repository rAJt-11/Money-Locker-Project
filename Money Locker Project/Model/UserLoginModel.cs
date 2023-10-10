using Newtonsoft.Json;

namespace MoneyLocker.Model
{
    public class UserLoginModel
    {
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
