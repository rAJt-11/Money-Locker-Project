using Newtonsoft.Json;
using System.Collections.Generic;

namespace MoneyLocker.Model
{
    public class ErrorInfo
    {
        [JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("errorList")]
        public List<ErrorDetailInfo> ErrorList { get; set; } = new List<ErrorDetailInfo>();

    }

    public class ErrorDetailInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("errorMsg")]
        public string ErrorMsg { get; set; }
    }
}
