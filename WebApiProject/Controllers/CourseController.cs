using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseBusiness _courseBusiness;
        private readonly IAccountBusiness _accountBusiness;

        public CourseController(ICourseBusiness courseBusiness, IAccountBusiness accountBusiness)
        {
            _courseBusiness = courseBusiness;
            _accountBusiness = accountBusiness;
        }

        [HttpGet]
        [Route("GetAllCourseByUser/{accountId}")]
        public async Task<IActionResult> GetCourseByUser([FromRoute]long accountId)
        {
            var account = await _accountBusiness.GetAccountById(accountId);
            if (account == null)
            {
                return NotFound();
            }

            var courses = await _courseBusiness.GetCourseByAccount(account);
            return Ok(courses);
        }
    }
}
