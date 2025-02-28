namespace IBankApp.Models
{
    public class DepositRequest
    {
        public DepositRequest(int amount, string message, string email)
        {
            this.amount = amount;
            this.message = message;
            this.email = email;
        }

        public int amount { get; set; }
        public string message { get; set; }
        public string email { get; set; }
    }
}
