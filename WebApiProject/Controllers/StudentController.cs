using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public ActionResult StudentProfile()
        {
            return View();


        }



    }
}
