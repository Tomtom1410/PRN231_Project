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

    public virtual ICollection<Class> Classes { get; } = new List<Class>();

    public virtual ICollection<Course> Courses { get; } = new List<Course>();

    public virtual ICollection<Document> Documents { get; } = new List<Document>();
}
