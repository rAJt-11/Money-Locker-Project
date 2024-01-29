using Newtonsoft.Json;

namespace MoneyLocker.Model
{
    public class ResponseInfo
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errorInfo")]
        public ErrorInfo ErrorInfo { get; set; }

    }
}
