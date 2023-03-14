using DataAccess.Dtos;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ICourseBusiness
    {
        Task <List<CourseDto>> GetCourseByAccount(Account account);
        Task <CourseDto> GetCourseById(long courseId);
    }
}
