using Newtonsoft.Json;

namespace MoneyLocker.Model.User
{
    public class UserDetails
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("mobile")]
        public long Mobile { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

    }
}
