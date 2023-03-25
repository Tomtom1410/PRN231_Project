using System.Net.Http.Headers;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebClientProject.Controllers
{
    public class FileController : BaseController
    {
        private const string _url = "https://localhost:7212/api/File/";
        private readonly HttpClient _httpClient = null;
        private readonly IWebHostEnvironment _environment;

        public FileController(IWebHostEnvironment webHost)
        {
            _environment = webHost;
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }
            ViewBag.LeftMenu = true;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            string downloadFolder = Path.Combine(_environment.WebRootPath, "Download");
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }

            ViewBag.LeftMenu = true;
            string fileExtention = Path.GetExtension(formFile.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtention;
            var pathFile = Path.Combine(_environment.WebRootPath, "Download", fileName);
            
            var document = new DocumentDto
            {
                AccountId = account.Id,
                CourseId = 1,
                PathFile = pathFile,
                DocumentName = fileName,
                ContentType = formFile.ContentType,
                DocumentOriginalName = formFile.FileName
            };
            using (var uploading = new FileStream(pathFile, FileMode.Create))
            {
                await formFile.CopyToAsync(uploading);
                ViewData["Message"] = "The Selected File " + formFile.FileName + " Is Saved success...";

                ViewBag.LeftMenu = true;
                var token = GetToken();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(_url + "SaveFileInformation", document);
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var courses = GetListCourseOfUser();
                        if (courses == null)
                        {
                            return View("Error");
                        }
                        ViewBag.Courses = courses;
                        return View();

                    case System.Net.HttpStatusCode.Conflict:
                        var fileInfo = new FileInfo(pathFile);
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
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
}