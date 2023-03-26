using BusinessLogic;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentBusiness documentBusiness;
        public DocumentController(IDocumentBusiness document)
        {
            documentBusiness = document;
           
        }
        [HttpGet]
        [Route("GetDocByUser/{accountId}")]
        public async Task<IActionResult> GetCourseByUser([FromRoute] long accountId)
        {
            var doc = await documentBusiness.GetDocumentsByUserAsync(accountId);
            if (doc == null)
            {
                return NotFound();
            }
            return Ok(doc);
        }

    }
}
