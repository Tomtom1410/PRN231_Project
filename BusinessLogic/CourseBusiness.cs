using System.Linq;
using Azure;
using BusinessLogic.Interfaces;
using DataAccess.Dtos;
using Repositories.Interfaces;
using Repositories.Models;

namespace BusinessLogic
{
    public class CourseBusiness : ICourseBusiness
    {
        private readonly ICourseRepository _courseRepository;

        public CourseBusiness(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<CourseDto>> GetCourseByAccount(Account account)
        {
            var listCourse = await _courseRepository.GetCoursesByAccount(account);

            var response = new List<CourseDto>();
            foreach (var course in listCourse)
            {
                var author = await _courseRepository.GetAuthorOfCourse(course.CourseId);
                var courseDto = new CourseDto()
                {
                    Id = course.CourseId,
                    CourseCode = course.Course?.CourseCode,
                    CourseName = course.Course?.CourseName,
                    Author = new AccountDto()
                    { 
                        Id = author.Id, 
                        Username = author.Username,
                        FullName = author.FullName,
                    },
                };
                response.Add(courseDto);
            }
            return response;
        }

        public async Task<CourseDto> GetCourseById(long courseId)
        {
            var course = await _courseRepository.GetCourseById(courseId);

            if (course == null)
            {
                return null;
            }
            var author = await _courseRepository.GetAuthorOfCourse(courseId);
            var students = await _courseRepository.GetStudentsOfCourse(courseId);

            var studentsResponse = students.Select(x => new AccountDto { Id = x.Id, FullName = x.FullName, Username = x.Username}).ToList();
            var response = new CourseDto()
            {
                Id = course.Id,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Author = new AccountDto()
                {
                    Id = author.Id,
                    Username = author.Username,
                    FullName = author.FullName,
                },
                Students = studentsResponse
            };

            return response;
        }

        public async Task<List<CourseDto>> SearchAsync(string textSearch)
        {
            var courses = await _courseRepository.SearchAsync(textSearch);

            var courseDtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                var author = await _courseRepository.GetAuthorOfCourse(course.Id);
                var courseDto = new CourseDto()
                {
                    Id = course.Id,
                    CourseCode = course.CourseCode,
                    CourseName = course.CourseName,
                    Author = new AccountDto()
                    {
                        Id = author.Id,
                        Username = author.Username,
                        FullName = author.FullName,
                    },
                };
                courseDtos.Add(courseDto);
            }

            return courseDtos;
        }

        public async Task<bool> EnrollCourseAsync(CourseAccount course)
        {
            return await _courseRepository.EnrollAsync(course);
        }

        public async Task<bool> UnEnrollCourseAsync(CourseAccount course)
        {
            return await _courseRepository.UnEnrollAsync(course);
        }
    }
}
