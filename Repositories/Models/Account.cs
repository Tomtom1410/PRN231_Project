using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Account
{
    public long Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public bool? IsTeacher { get; set; }

    public string? FullName { get; set; }

    public DateTime? Dob { get; set; }

    public virtual ICollection<CourseAccount> CourseAccounts { get; } = new List<CourseAccount>();

    public virtual ICollection<Document> Documents { get; } = new List<Document>();

    public virtual ICollection<Class> Classes { get; } = new List<Class>();
}
