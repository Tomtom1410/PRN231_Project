﻿using System.Net.Http.Headers;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace WebClientProject.Controllers
{
    public class FileController : BaseController
    {
        private const string _url = "https://localhost:7212/api/File/";
        private const string _urlCourse = "https://localhost:7212/api/Course/";

        private readonly HttpClient _httpClient = null;
        private readonly IWebHostEnvironment _environment;
        private const string FOLDER_NAME = "Download";

        public FileController(IWebHostEnvironment webHost)
        {
            _environment = webHost;
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [HttpGet]
        public async Task<IActionResult> Upload(int id)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }
            ViewBag.LeftMenu = true;

            var course = await GetCourseByIdAsync(id);
            var courses = await GetListCourseOfUserAsync();
            if (courses == null || course == null)
            {
                return View("Error");
            }
            ViewBag.Courses = courses;
            ViewBag.CourseUploadId = course.Id;
            return View();
        }

        private async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            
            HttpResponseMessage responseMessage = await httpClient.GetAsync(_urlCourse + id);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<CourseDto>();
                    return response;
                default:
                    return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile, long courseId)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            string downloadFolder = Path.Combine(_environment.WebRootPath, FOLDER_NAME);
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }

            ViewBag.LeftMenu = true;
            ViewBag.CourseUploadId = courseId;
            string fileExtention = Path.GetExtension(formFile.FileName);
            var fileName = Guid.NewGuid().ToString() + fileExtention;
            var pathFile = Path.Combine(_environment.WebRootPath, FOLDER_NAME, fileName);

            var document = new DocumentDto
            {
                AccountId = account.Id,
                CourseId = courseId,
                PathFile = pathFile,
                DocumentName = fileName,
                ContentType = formFile.ContentType,
                DocumentOriginalName = formFile.FileName
            };
            using (var uploading = new FileStream(pathFile, FileMode.Create))
            {
                await formFile.CopyToAsync(uploading);
                uploading.Close();
                ViewData["Message"] = "The Selected File " + formFile.FileName + " Is Saved success...";

                ViewBag.LeftMenu = true;
                
                HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(_url + "SaveFileInformation", document);
                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var courses = await GetListCourseOfUserAsync();
                        if (courses == null)
                        {
                            return View("Error");
                        }
                        ViewBag.Courses = courses;
                        return View();

                    case System.Net.HttpStatusCode.Conflict:
                        if (System.IO.File.Exists(pathFile))
                        {
                            System.IO.File.Delete(pathFile);
                        }
                        return View("Error");

                    case System.Net.HttpStatusCode.Forbidden:
                        return StatusCode(StatusCodes.Status403Forbidden);

                    case System.Net.HttpStatusCode.Unauthorized:
                        return Redirect("../Auth/Login");
                }
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAsync(string fileName, string contentType)
        {
            var account = GetSession();
			if (account == null)
			{
				return Redirect("../Auth/Login");
			}
			string downloadFolder = Path.Combine(_environment.WebRootPath, FOLDER_NAME);

            string filePath = Path.Combine(downloadFolder, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Đọc tệp tin vào bộ nhớ
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            // Trả về File để download
            return File(memory, contentType, Path.GetFileName(filePath));
        }

    }
}