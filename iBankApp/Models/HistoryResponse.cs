namespace IBankApp.Models
{
    public class HistoryResponse
    {
        public List<Statement> statement { get; set; }
        public string error { get; set; }

        public class Statement 
        {
            public int amount { get; set; }
            public DateTime dateTime { get; set; }
            public FromAccount fromAccount { get; set; }
            public string message { get; set; }
            public ToAccount toAccount { get; set; }
            public string transactionId { get; set; }
            public string type { get; set; }



            public class FromAccount
            {
                public string accountId { get; set; }
                public string email { get; set; }
                public string firstName { get; set; }
                public string lastName { get; set; }
                public string mobilePhone { get; set; }
            }

            public class ToAccount
            {
                public string accountId { get; set; }
                public string email { get; set; }
                public string firstName { get; set; }
                public string lastName { get; set; }
                public string mobilePhone { get; set; }
            }
        }
    }
}
