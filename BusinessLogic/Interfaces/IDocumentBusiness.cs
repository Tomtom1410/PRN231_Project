using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Dtos;
using Repositories.Models;

namespace BusinessLogic.Interfaces
{
	public interface IDocumentBusiness
	{
		Task <List<DocumentDto>> DeleteDocumentAsync(List<DocumentDto> documents);
		Task<List<DocumentDto>> GetDocumentsByCourseAsync(long courseId);
		Task<List<DocumentDto>> GetDocumentsByUserAsync(long courseId);
		Task<bool> saveFileInfoAsync(Document documentEntity);
	}
}
