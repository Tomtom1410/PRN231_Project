using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories
{
	public class DocumentRepository :IDocumentRepository
	{
        private readonly Prn231ProjectContext _dbContext;
        private readonly ILogger<DocumentRepository> _logger;

        public DocumentRepository(Prn231ProjectContext dbContext, ILogger<DocumentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Document>> GetDocumentsByCourseAsync(long courseId)
        {
            return await _dbContext.Documents
                .Include(x => x.Account)
                .Where(x => x.CourseId == courseId && x.Account.IsTeacher == true).ToListAsync();
        }

        public async Task<List<Document>> GetDocumentsByUserAsync(long id)
        {
            return await _dbContext.Documents
                .Where(x => x.AccountId == id).ToListAsync();
        }

        public async Task<bool> SaveFileInfoAsync(Document documentEntity)
        {
            try
            {
                await _dbContext.Documents.AddAsync(documentEntity);
                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }catch(Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }
    }
}
