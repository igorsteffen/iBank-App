namespace IBankApp.Models
{
    public class AllAccountsResponse
    {
        public List<Accounts> accounts { get; set; }
        public string error { get; set; }

        public class Accounts()
        {
            public string accountId { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string mobilePhone { get; set; }
        }
    }
}
