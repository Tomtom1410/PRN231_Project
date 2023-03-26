using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
	public interface IDocumentRepository
	{
		Task<List<Document>> DeleteDocumentAsync(List<Document> documentDelete);
		Task<List<Document>> GetDocumentsByCourseAsync(long courseId);
		Task<bool> SaveFileInfoAsync(Document documentEntity);
	}
}
