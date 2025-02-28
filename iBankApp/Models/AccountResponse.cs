using static IBankApp.Models.AllAccountsResponse;

namespace IBankApp.Models
{
    public class AccountResponse
    {

        public AccountItem account { get; set; }
        public string error { get; set; }


        public class AccountItem
        {
            public string accountId { get; set; }
            public string email { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string mobilePhone { get; set; }
        }

        
    }
}
