using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public TeacherController(IWebHostEnvironment webHost)
        {
            _environment = webHost;
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile formFile)
        {
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
