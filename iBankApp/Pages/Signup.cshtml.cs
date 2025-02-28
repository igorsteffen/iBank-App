using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IBankApp.Models;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace IBankApp.Pages
{
    public class SignupModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public SignupModel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        [BindProperty]
        public string FirstName { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string MobilePhone { get; set; }
        [BindProperty]
        public string Password { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var requestBody = new SignupRequest(FirstName, LastName, Email, int.Parse(MobilePhone), Password);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://starfish-app-g96va.ondigitalocean.app/v1/accounts/create", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var signupResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                    return RedirectToPage("/Activation");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var signupResponse = JsonConvert.DeserializeObject<SignupResponse>(errorContent);
                    ErrorMessage = $"Error on SignUp: {signupResponse?.error}";
                    return Page();
                }

            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error calling the API: {ex.Message}";
                return Page();
            }
        }
    }
}
