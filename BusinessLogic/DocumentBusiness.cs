using BusinessLogic.Interfaces;
using DataAccess.Dtos;
using Repositories.Interfaces;
using Repositories.Models;

namespace BusinessLogic
{
    public class DocumentBusiness : IDocumentBusiness
    {
        private readonly IDocumentRepository _documentRepository;

        public DocumentBusiness(IDocumentRepository repository)
        {
            _documentRepository = repository;
        }

        public async Task<List<DocumentDto>> GetDocumentsByCourseAsync(long courseId)
        {
            var documents = await _documentRepository.GetDocumentsByCourseAsync(courseId);
            var response = documents.Select(x => new DocumentDto {
                Id = x.Id,
                PathFile = x.PathFile,
                DocumentOriginalName = x.DocumentOriginalName,
                DocumentName = x.DocumentName,
                ContentType = x.ContentType,
                AccountId = x.AccountId,
                CourseId = courseId,
            }).ToList();
            return response;
        }

        public async Task<bool> saveFileInfoAsync(Document documentEntity)
        {
            return await _documentRepository.SaveFileInfoAsync(documentEntity);
        }
    }
}