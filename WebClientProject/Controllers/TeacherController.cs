using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;

namespace WebClientProject.Controllers
{
    public class TeacherController : BaseController
    {
        private const string _url = "https://localhost:7212/api/Teacher/";
        private readonly HttpClient _httpClient = null;
        private readonly IWebHostEnvironment _environment;

        public TeacherController(IWebHostEnvironment webHost)
        {
            _environment = webHost;
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.LeftMenu = true;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile formFile)
        {   
            var contenType = formFile.ContentType.ToLower();
            Console.WriteLine(contenType);

            ViewBag.LeftMenu = true;
            var saveFile = Path.Combine(_environment.WebRootPath, "Download", formFile.FileName);
            string file = Path.GetExtension(formFile.FileName);
            if (file == ".doc" || file == ".jpg")
            {
                using (var uploading = new FileStream(saveFile, FileMode.Create))
                {
                    await formFile.CopyToAsync(uploading);
                    ViewData["Message"] = "The Selected File " + formFile.FileName + " Is Saved success...";
                }
            }
            else
            {
                ViewData["Message"] = " Only the file type .doc and jpg";
            }
            return View();
        }


    }
}
