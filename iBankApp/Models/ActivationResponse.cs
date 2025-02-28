namespace IBankApp.Models
{
    public class ActivationResponse
    {
        public ActivationResponse(string result, string error)
        {
            this.result = result;
            this.error = error;
        }

        public string result { get; set; }
        public string error { get; set; }
    }
}
