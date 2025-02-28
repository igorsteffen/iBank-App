namespace IBankApp.Models
{
    public class SignupRequest
    {
        public SignupRequest(string firstName, string lastName, string email, int mobilePhone, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.mobilePhone = mobilePhone;
            this.password = password;
        }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int mobilePhone { get; set; }
        public string password { get; set; }
    }
}
