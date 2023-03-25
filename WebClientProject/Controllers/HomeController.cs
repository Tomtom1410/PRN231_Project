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
        private const string _url = "";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.LeftMenu = true;
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            var courses = await GetListCourseOfUserAsync();
            ViewBag.Courses = courses;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("../Auth/Login");
        }
    }
}
