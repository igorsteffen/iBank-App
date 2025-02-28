using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IBankApp.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static IBankApp.Models.AccountResponse;
using static IBankApp.Models.HistoryResponse;

namespace IBankApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        [BindProperty]
        public AccountItem Account { get; set; } = new AccountItem();
        [BindProperty]
        public BalanceResponse Balance { get; set; }
        public List<Statement> Statements { get; set; } = new List<Statement>();
        public string ErrorMessage { get; set; }

        public IndexModel(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IActionResult> OnGet()
        {
            var token = GetToken();
            if (token == null)
            {
                return RedirectToPage("/Login");
            }

            var email = HttpContext.Session.GetString("user_email");
            Account = await FetchAccountAsync($"https://starfish-app-g96va.ondigitalocean.app/v1/accounts/info/{email}", token);
            Balance = await FetchBalanceAsync($"https://starfish-app-g96va.ondigitalocean.app/v1/accounts/balance", token);
            Statements = await FetchStatementAsync($"https://starfish-app-g96va.ondigitalocean.app/v1/transactions/25/statement", token);
            return Page();
        }

        public async Task<AccountItem> FetchAccountAsync(string url, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var accountResponse = JsonConvert.DeserializeObject<AccountResponse>(responseString);
                    return accountResponse.account;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var accountResponse = JsonConvert.DeserializeObject<AccountResponse>(responseString);
                    return new AccountItem();
                }
            }
            catch (Exception ex)
            {
                return new AccountItem();
            }
        }

        public async Task<BalanceResponse> FetchBalanceAsync(string url, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var balanceResponse = JsonConvert.DeserializeObject<BalanceResponse>(responseString);
                    return balanceResponse;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var balanceResponse = JsonConvert.DeserializeObject<BalanceResponse>(responseString);
                    ErrorMessage = balanceResponse.error;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error fetching balance";
            }

            return new BalanceResponse();
        }

        public async Task<List<Statement>> FetchStatementAsync(string url, string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-access-token", token);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statementResponse = JsonConvert.DeserializeObject<HistoryResponse>(responseString);
                    return statementResponse.statement;
                }
                else
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var statementResponse = JsonConvert.DeserializeObject<HistoryResponse>(responseString);
                    ErrorMessage = statementResponse.error;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error fetching statements";
            }

            return new List<Statement>();
        }

        public async Task<IActionResult> OnPostLogout()
        {
            HttpContext.Session.Remove("auth_token");
            HttpContext.Session.Remove("user_email");
            return RedirectToPage("/Login");
        }

        private string GetToken()
        {
            return HttpContext.Session.GetString("auth_token");
        }
    }
}
