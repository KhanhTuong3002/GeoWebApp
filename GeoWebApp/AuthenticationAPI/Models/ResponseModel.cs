namespace AuthenticationAPI.Models
{
    public class ResponseModel
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public bool isAdmin { get; set; }
    }
}
