using Newtonsoft.Json;

namespace MoneyLocker.Model.User
{
    public class UserLogin
    {
        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

}
