using BusinessLogic.Interfaces;
using DataAccess.Dtos;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IDocumentBusiness _documentBusiness;
        private readonly ICourseBusiness _courseBusiness;

        public FileController(IDocumentBusiness document, ICourseBusiness courseBusiness)
        {
            _documentBusiness = document;
            _courseBusiness = courseBusiness;
        }

        [HttpPost]
        [Route("SaveFileInformation")]
        public async Task<IActionResult> SaveFileInformation([FromBody] DocumentDto document)
        {
            var documentEntity = new Document
            {
                AccountId = document.AccountId,
                CourseId = document.CourseId,
                DocumentName = document.DocumentName,
                DocumentOriginalName = document.DocumentOriginalName,
                ContentType = document.ContentType,
                PathFile = document.PathFile,
            };

            var result = await _documentBusiness.saveFileInfoAsync(documentEntity);
            if (!result)
            {
                return Conflict();
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetDocumentsByCourse/{courseId}")]
        public async Task<IActionResult> GetDocumentsByCourse([FromRoute] long courseId)
        {
            var course = await _courseBusiness.GetCourseById(courseId);
            if (course == null)
            {
                return NotFound("Not Found Course");
            }

            var document = await _documentBusiness.GetDocumentsByCourseAsync(courseId);
            return Ok(document);
        }

        [HttpPost]
        [Route("DeleteDocuments")]
        public async Task<IActionResult> DeleteDocuments([FromBody] List<DocumentDto> documents)
        {
            var response = await _documentBusiness.DeleteDocumentAsync(documents);
            if(response == null)
            {
                return Conflict();
            }
            return Ok(response);
        } 
    } 
}