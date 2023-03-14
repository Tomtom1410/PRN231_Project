using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace WebClientProject.Controllers
{
    public class HomeController : BaseController
    {
        private const string _url = "https://localhost:7212/api/Course/";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.LeftMenu = true;
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }
            
            var token = GetToken();
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _url + $"GetAllCourseByUser/{account.Id}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + token);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(request);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<List<CourseDto>>();
                   
                    return View();
                case System.Net.HttpStatusCode.NotFound:
                    ViewData["msg"] = "Username or password is in valid. Please try again!";
                    return View("Error");
                case System.Net.HttpStatusCode.Forbidden:
                    return Forbid();
                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }

            return View();
        }
    }
}
