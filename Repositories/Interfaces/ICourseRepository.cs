using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICourseRepository
    {
        Task<Account> GetAuthorOfCourse(long courseId);
        Task <Course> GetCourseById(long courseId);
        Task<List<CourseAccount>> GetCoursesByAccount(Account account);
        Task<List<Account>> GetStudentsOfCourse(long courseId);
    }
}
