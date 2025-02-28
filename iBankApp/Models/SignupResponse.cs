namespace IBankApp.Models
{
    public class SignupResponse
    {
        public SignupResponse(string accountId, string error)
        {
            this.accountId = accountId;
            this.error = error;
        }

        public string accountId { get; set; }
        public string error { get; set; }
    }
}
