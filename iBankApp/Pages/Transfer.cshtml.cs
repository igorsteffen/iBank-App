using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IBankApp.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using static IBankApp.Models.AllAccountsResponse;

namespace IBankApp.Pages
{
    public class TransferModel : PageModel
    {
        private readonly HttpClient _httpClient;
        public TransferModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Accounts> Accounts { get; set; } = new List<Accounts>();
        public string ErrorMessage { get; set; }
        [BindProperty]
        public int Amount { get; set; }
        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public string Account { get; set; }

        public async Task<IActionResult> OnGet()
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

                var response = await _httpClient.GetAsync("https://starfish-app-g96va.ondigitalocean.app/v1/accounts/info");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var accountsResponse = JsonConvert.DeserializeObject<AllAccountsResponse>(responseString);
                    Accounts = accountsResponse.accounts;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var accountsResponse = JsonConvert.DeserializeObject<AllAccountsResponse>(responseString);
                    ErrorMessage = accountsResponse.error;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error fetching accounts";
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

                var requestBody = new TransferRequest(Amount, Message, Account);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("https://starfish-app-g96va.ondigitalocean.app/v1/transactions/transfer/", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var transferResponse = JsonConvert.DeserializeObject<TransferResponse>(responseString);
                    ErrorMessage = transferResponse.error;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error on Transfer transaction!";
            }
            return Page();
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
