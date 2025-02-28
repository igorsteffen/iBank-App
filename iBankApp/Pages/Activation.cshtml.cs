using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IBankApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace IBankApp.Pages
{
    public class ActivationModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public ActivationModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string ActivationCode { get; set; }
        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                var requestBody = new ActivationRequest(Email, ActivationCode);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://starfish-app-g96va.ondigitalocean.app/v1/accounts/activate", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var activationResponse = JsonConvert.DeserializeObject<ActivationResponse>(responseString);
                    return RedirectToPage("/Login");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var activationResponse = JsonConvert.DeserializeObject<ActivationResponse>(errorContent);
                    ErrorMessage = $"Error on activation: {activationResponse?.error}";
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
