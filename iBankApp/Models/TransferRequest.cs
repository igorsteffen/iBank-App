namespace IBankApp.Models
{
    public class TransferRequest
    {
        public TransferRequest(int amount, string message, string account)
        {
            this.amount = amount;
            this.message = message;
            this.account = account;
        }

        public int amount { get; set; }
        public string message { get; set; }
        public string account { get; set; }
    }
}
