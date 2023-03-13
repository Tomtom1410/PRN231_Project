namespace DataAccess.Dtos
{
    public class AccountDto
    {
        public long? Id { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public bool? IsTeacher { get; set; }

        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }
        
    }
}
