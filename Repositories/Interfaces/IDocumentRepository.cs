
using Repositories.Models;

namespace Repositories.Interfaces
{
	public interface IDocumentRepository
	{
		Task<List<Document>> DeleteDocumentAsync(List<Document> documentDelete);
		Task<List<Document>> GetDocumentsByCourseAsync(long courseId);
		Task<List<Document>> GetDocumentsByStudentAsync(long courseId);
		Task<bool> SaveFileInfoAsync(Document documentEntity);
        Task<List<Document>> GetDocumentsByUserAsync(long id, long courseId);
    }
}
