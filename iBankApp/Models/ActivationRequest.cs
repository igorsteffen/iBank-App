namespace IBankApp.Models
{
    public class ActivationRequest
    {
        public ActivationRequest(string email, string activationCode)
        {
            this.email = email;
            this.activationCode = activationCode;
        }

        public string email { get; set; }
        public string activationCode { get; set; }
    }
}
