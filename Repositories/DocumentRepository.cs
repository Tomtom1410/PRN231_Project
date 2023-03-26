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

        public async Task<List<Document>> DeleteDocumentAsync(List<Document> documentDelete)
        {
            var entities = new List<Document>();
            foreach (var document in documentDelete)
            {
                var d = await _dbContext.Documents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == document.Id);
                if (d == null)
                {
                    return null;
                }
                entities.Add(d);
            }

            if(entities.Count < 0)
            {
                return null;
            }

            var transition = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                _dbContext.Documents.RemoveRange(entities);
                var result = await _dbContext.SaveChangesAsync();
                await transition.CommitAsync();
                if(result > 0)
                {
                    return entities;
                }

                return null;
            }catch (Exception e)
            {
                _logger.LogError(e.Message);
                await transition.RollbackAsync();
                return null;
            }
        }

        public async Task<List<Document>> GetDocumentsByCourseAsync(long courseId)
        {
            return await _dbContext.Documents
                .Include(x => x.Account)
                .Where(x => x.CourseId == courseId && x.Account.IsTeacher == true).ToListAsync();
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
