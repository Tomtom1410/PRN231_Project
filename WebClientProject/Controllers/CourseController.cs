using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WebClientProject.Controllers
{
    public class CourseController : BaseController
    {
        private const string _url = "https://localhost:7212/api/Course/";
        public async Task<IActionResult> Details(long id)
        {
            ViewBag.LeftMenu = true;
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(_url + id);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<CourseDto>();
                    var courses = GetListCourseOfUser();
                    if (courses == null)
                    {
                        return View("Error");
                    }
                    ViewBag.Courses = courses;
                    return View(response);
                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();
                case System.Net.HttpStatusCode.Forbidden:
                    return Forbid();
                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }
    }
}
