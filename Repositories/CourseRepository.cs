using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CourseRepository> _logger;

        public CourseRepository(Prn231ProjectContext dbContext, ILogger<CourseRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> EnrollAsync(CourseAccount course)
        {
            try
            {
                await _dbContext.CourseAccounts.AddAsync(course);
                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
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

        public async Task<List<Course>> SearchAsync(string textSearch)
        {
            return await _dbContext.Courses
                .Where(x => x.CourseName.ToLower().Contains(textSearch.ToLower())
                || x.CourseCode.ToLower().Contains(textSearch.ToLower()))
                .OrderBy(x => x.CourseName)
                .ToListAsync();
        }

        public async Task<bool> UnEnrollAsync(CourseAccount course)
        {
            var transition = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                 _dbContext.CourseAccounts.Remove(course);
                var result = await _dbContext.SaveChangesAsync();
                await transition.CommitAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await transition.RollbackAsync();
                return false;
            }
        }
    }
}
