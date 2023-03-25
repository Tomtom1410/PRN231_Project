namespace DataAccess.Dtos
{
    public class CourseDto
    {
        public long? Id { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
        public AccountDto? Author { get; set; }
        public List<AccountDto>? Students { get; set; }
    }
}
