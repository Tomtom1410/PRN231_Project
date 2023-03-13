using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WebClientProject.Controllers
{
    public class AuthController : BaseController
    {
        private const string _url = "https://localhost:7212/api/Auth/";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync(_url + "Login", model);

            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var response = await responseMessage.Content.ReadFromJsonAsync<LoginModelResponse>();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.Token);
                    SetSession(response.Account);
                    return Redirect("Test");
                case System.Net.HttpStatusCode.NotFound:
                    ViewData["msg"] = "Username or password is in valid. Please try again!";
                    return View(model);
            }

            return View(model);
        }
    }
}
