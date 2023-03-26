using System.Net.Http.Headers;
using System.Reflection;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace WebClientProject.Controllers
{
    public class CourseController : BaseController
    {
        private const string _url = "https://localhost:7212/api/Course/";
        private const string _urlDocument = "https://localhost:7212/api/File/";
        private const int pageSize = 4;

        public async Task<IActionResult> Index(string txtSearch, int page = 1)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            ViewBag.LeftMenu = true;
            if (string.IsNullOrEmpty(txtSearch))
            {
                return Redirect("../Home");
            }
            var courses = await GetListCourseOfUserAsync();
            if (courses == null)
            {
                return View("Error");
            }

            ViewBag.Courses = courses;

            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(_url + $"Search/{txtSearch}");
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<List<CourseDto>>();
                    ViewBag.txtSearch = txtSearch;
                    ViewData["textSearch"] = txtSearch;
                    ViewBag.currentPage = page;
                    ViewBag.TotalPage = (int)Math.Ceiling((double)response.Count() / pageSize); ;
                    return View(response.Skip((page - 1) * pageSize).Take(pageSize).ToList());

                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();

                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);

                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }

        public async Task<IActionResult> Details(long id)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            ViewBag.LeftMenu = true;
            ViewBag.currentUser = account; 
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(_url + id);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<CourseDto>();
                    var courses = await GetListCourseOfUserAsync();
                    if (courses == null)
                    {
                        return View("Error");
                    }
                    ViewBag.Documents = await GetDocumentOfCourse(id);
                    ViewBag.Courses = courses;
                    return View(response);

                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();

                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);

                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }

        private async Task<List<DocumentDto>> GetDocumentOfCourse(long id)
        {
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = _urlDocument + $"GetDocumentsByCourse/{id}";
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<List<DocumentDto>>();
                    return response;
                default:
                    return null;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Enroll(int id)
        {
            var account = GetSession();
            if(account == null)
            {
                return Redirect("../Auth/Login");
            }
            ViewBag.LeftMenu = true;
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(_url + id);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<CourseDto>();
                    var courses = await GetListCourseOfUserAsync();
                    if (courses == null)
                    {
                        return View("Error");
                    }
                    ViewBag.Documents = await GetDocumentOfCourse(id);
                    ViewBag.Courses = courses;
                    var isEnroll = response.Students.Any(x => x.Id == account.Id);
                    if (isEnroll || account.IsTeacher == true)
                    {
                        return Redirect($"../Details/{id}");
                    }

                    return View(response);

                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();

                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);

                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(CourseDto model)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }
            var courseAcount = new CourseAccountDto
            {
                AccountId = (long)account.Id,
                CourseId = (long)model.Id,
                IsAuthor = false
            };
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(_url + "Enroll", courseAcount);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return Redirect($"../Details/{model.Id}");
                case System.Net.HttpStatusCode.Conflict:
                    return View("Error");
                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);
                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UnEnroll(long id)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }
            var courseAcount = new CourseAccountDto
            {
                AccountId = (long)account.Id,
                CourseId = id,
                IsAuthor = false
            };
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(_url + "UnEnroll", courseAcount);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return Redirect($"../../Home");
                case System.Net.HttpStatusCode.Conflict:
                    return View("Error");
                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);
                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }

        

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            if (account.IsTeacher == false)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            ViewBag.LeftMenu = true;
            ViewBag.currentUser = account;
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.GetAsync(_url + id);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<CourseDto>();
                    if (response.Author.Id != account.Id)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden);
                    }
                    var courses = await GetListCourseOfUserAsync();
                    if (courses == null)
                    {
                        return View("Error");
                    }

                    ViewBag.Documents = await GetDocumentOfCourse(id);
                    ViewBag.Courses = courses;
                    return View(response);

                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();

                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);

                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long[] documents)
        {
            var account = GetSession();
            if (account == null)
            {
                return Redirect("../Auth/Login");
            }

            var documentList = new List<DocumentDto>();
            foreach (var document in documents)
            {
                documentList.Add(new DocumentDto { Id = document});
            } ;
            var token = GetToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(_urlDocument + "DeleteDocuments", documentList);
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<List<DocumentDto>>();
                    if(response != null)
                    {
                        foreach (var document in response)
                        {
                            if (System.IO.File.Exists(document.PathFile))
                            {
                                System.IO.File.Delete(document.PathFile);
                            }
                        }
                    }
                    return Json("success");

                case System.Net.HttpStatusCode.NotFound:
                    return NotFound();

                case System.Net.HttpStatusCode.Forbidden:
                    return StatusCode(StatusCodes.Status403Forbidden);

                case System.Net.HttpStatusCode.Unauthorized:
                    return Redirect("../Auth/Login");
            }

            return Json("Failed");
        }
    }
}