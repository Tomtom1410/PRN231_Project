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

        public async Task<Account> GetAuthorOfCourse(long courseId)
        {
            var courseAccount = await _dbContext.CourseAccounts.Include(x => x.Account).FirstOrDefaultAsync(x => x.CourseId == courseId && x.IsAuthor == true);

            return courseAccount?.Account;
        }

        public async Task<Course> GetCourseById(long courseId)
        {
            return await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == courseId);
        }

        public async Task<List<CourseAccount>> GetCoursesByAccount(Account account)
        {
            var query = _dbContext.CourseAccounts
                .Include(x => x.Course)
                .Where(x => x.AccountId == account.Id);

            if(account.IsTeacher == true)
            {
                return await query.Where(x => x.IsAuthor == true).ToListAsync();
            }

            return await query.Where(x => x.IsAuthor == false).ToListAsync();
        }

        public async Task<List<Account>> GetStudentsOfCourse(long courseId)
        {
            var courseAccount = await _dbContext.CourseAccounts
                .Include(x => x.Account)
                .Where(x => x.CourseId == courseId && x.IsAuthor == false).ToListAsync();

            return courseAccount.Select(x => x.Account).ToList();
        }
    }
}
