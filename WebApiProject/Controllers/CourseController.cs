using BusinessLogic.Interfaces;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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

        [HttpGet]
        [Route("{courseId}")]
        public async Task<IActionResult> GetCourseById([FromRoute] long courseId)
        {
            var course = await _courseBusiness.GetCourseById(courseId);

            if(course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpGet]
        [Route("Search/{textSearch}")]
        public async Task<IActionResult> SearchAsync([FromRoute] string textSearch)
        {
            var courses = await _courseBusiness.SearchAsync(textSearch);
            return Ok(courses);
        }

        [HttpPost]
        [Route("Enroll")]
        public async Task<IActionResult> Enroll([FromBody] CourseAccountDto model)
        {
            var course = new CourseAccount
            {
                AccountId = model.AccountId,
                CourseId = model.CourseId,
                IsAuthor = model.IsAuthor,
            };
            var result = await _courseBusiness.EnrollCourseAsync(course);
            if (!result)
            {
                return Conflict();
            }
            return Ok();
        }

        [HttpPost]
        [Route("UnEnroll")]
        public async Task<IActionResult> UnEnroll([FromBody] CourseAccountDto model)
        {
            var course = new CourseAccount
            {
                AccountId = model.AccountId,
                CourseId = model.CourseId,
                IsAuthor = model.IsAuthor,
            };
            var result = await _courseBusiness.UnEnrollCourseAsync(course);
            if (!result)
            {
                return Conflict();
            }
            return Ok();
        }
    }
}
