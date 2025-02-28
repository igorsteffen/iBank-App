using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IBankApp.Models;
using Newtonsoft.Json;
using static IBankApp.Models.AllAccountsResponse;
using System.Text;

namespace IBankApp.Pages
{
    public class WithdrawModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public WithdrawModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public string ErrorMessage { get; set; }
        [BindProperty]
        public int Amount { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var requestBody = new WithdrawRequest(Amount);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://starfish-app-g96va.ondigitalocean.app/v1/transactions/withdraw", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var withdrawResponse = JsonConvert.DeserializeObject<WithdrawResponse>(responseString);
                    ErrorMessage = withdrawResponse.error;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error on Withdraw transaction!";
            }
            return Page();
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
