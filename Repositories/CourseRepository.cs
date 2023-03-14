using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly Prn231ProjectContext _dbContext;

        public CourseRepository(Prn231ProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CourseAccount>> GetCoursesByAccount(Account account)
        {
            var query = _dbContext.CourseAccounts
                .Include(x => x.Account)
                .Include(x => x.Course)
                .Where(x => x.AccountId == account.Id);

            if(account.IsTeacher == true)
            {
                return await query.Where(x => x.IsAuthor == true).ToListAsync();
            }

            return await query.Where(x => x.IsAuthor == false).ToListAsync();
        }
    }
}
