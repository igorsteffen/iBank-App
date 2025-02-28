namespace IBankApp.Models
{
    public class WithdrawRequest
    {
        public WithdrawRequest(int amount)
        {
            this.amount = amount;
        }

        public int amount { get; set; }
    }
}
