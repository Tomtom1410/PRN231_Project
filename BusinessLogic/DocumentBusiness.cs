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

        public async Task<List<DocumentDto>> DeleteDocumentAsync(List<DocumentDto> documents)
        {
            var documentDelete = documents.Select(x => new Document { Id = x.Id}).ToList();
            var documentReponse = await _documentRepository.DeleteDocumentAsync(documentDelete);
            if (documentReponse != null && documentReponse.Count > 0)
            {
                return documentReponse
                    .Select(x => new DocumentDto { Id = x.Id, DocumentName = x.DocumentName, PathFile = x.PathFile})
                    .ToList();
            }
            return null;
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

        public async Task<List<DocumentDto>> GetDocumentsByStudentAsync(long courseId)
        {
            var documents = await _documentRepository.GetDocumentsByStudentAsync(courseId);
            var response = documents.Select(x => new DocumentDto {
                Id = x.Id,
                PathFile = x.PathFile,
                DocumentOriginalName = x.DocumentOriginalName,
                DocumentName = x.DocumentName,
                ContentType = x.ContentType,
                AccountId = x.AccountId,
                CourseId = courseId,
				Author  = new AccountDto
                {
                    FullName = x.Account?.FullName,
                }

			}).ToList();
            return response;
        }

        public async Task<List<DocumentDto>> GetDocumentsByUserAsync(long userId, long courseId)
        {

            var documents = await _documentRepository.GetDocumentsByUserAsync(userId, courseId);
            var response = documents.Select(x => new DocumentDto
            {
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