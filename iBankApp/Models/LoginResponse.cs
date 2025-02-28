namespace IBankApp.Models
{
    public class LoginResponse
    {
        public LoginResponse(string expiration, string token, string error)
        {
            this.expiration = expiration;
            this.token = token;
            this.error = error;
        }

        public string expiration { get; set; }
        public string token { get; set; }
        public string error { get; set; }
    }
}
