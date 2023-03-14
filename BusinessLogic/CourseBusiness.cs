using BusinessLogic.Interfaces;
using DataAccess.Dtos;
using Repositories.Interfaces;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CourseBusiness : ICourseBusiness
    {
        private readonly ICourseRepository _courseRepository;

        public CourseBusiness (ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CourseDto>> GetCourseByAccount(Account account)
        {
            var listCourse = await _courseRepository.GetCoursesByAccount(account);

            var response = new List<CourseDto>();
            foreach (var course in listCourse) {
                var courseDto = new CourseDto()
                {
                    Id = course.CourseId,
                    CourseCode = course.Course?.CourseCode,
                    CourseName = course.Course?.CourseName,
                    Author = new AccountDto()
                    {
                        Id = account.Id,
                        FullName = account.FullName,
                        IsTeacher = account.IsTeacher,
                    }
                };
                response.Add(courseDto);
            }
            return response;
        }
    }
}
