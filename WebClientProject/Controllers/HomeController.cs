using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _url + $"GetAllCourseByUser/{account.Id}";
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<List<CourseDto>>();
                    SetListCourseOfUser(response);
                    ViewBag.Courses = response;
                    return View();
                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();
                case System.Net.HttpStatusCode.Forbidden:
                    return Forbid();
                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }

            return View();
        }

        private void SetListCourseOfUser(List<CourseDto> courses)
        {
            HttpContext.Session.SetString("listCourse", JsonSerializer.Serialize(courses));
        }
    }
}
