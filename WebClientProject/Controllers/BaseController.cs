using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClientProject.Controllers
{
    public class BaseController : Controller
    {
        protected HttpClient httpClient = null;

        public BaseController()
        {
            httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }
        protected void SetSession(AccountDto account)
        {
            HttpContext.Session.SetString("account", JsonSerializer.Serialize(account));
        }

        protected AccountDto GetSession()
        {
            string accountValue = HttpContext.Session.GetString("account");
            if ( accountValue == null)
            {
                return null;
            }

            var account = JsonSerializer.Deserialize<AccountDto>(accountValue);
            return account;
        }
    }
}
