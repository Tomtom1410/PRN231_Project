using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
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

        protected string GetToken()
        {
            return HttpContext.Session.GetString("AccessToken");
        }

        protected async Task<List<CourseDto>> GetListCourseOfUserAsync()
        {
            var account = GetSession();
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url =$"https://localhost:7212/api/Course/GetAllCourseByUser/{account.Id}";
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<List<CourseDto>>();
                    return response;
                default:
                    return null;

            }
        }
    }
}
