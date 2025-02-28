using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IBankApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace IBankApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public LoginModel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        [BindProperty]
        public string Email { get; set; }
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
                var requestBody = new LoginRequest(Email, Password);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://starfish-app-g96va.ondigitalocean.app/v1/accounts/auth", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseString);
                    HttpContext.Session.SetString("auth_token", loginResponse.token);
                    HttpContext.Session.SetString("user_email", Email);
                    return RedirectToPage("/Index");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(errorContent);
                    ErrorMessage = $"Error on login: {loginResponse?.error}";
                    if(loginResponse.error == "The account is not activated yet")
                    {
                        return RedirectToPage("/Activation");
                    }
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
