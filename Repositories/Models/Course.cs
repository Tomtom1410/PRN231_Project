using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Course
{
    public long Id { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseName { get; set; }

    public virtual ICollection<CourseAccount> CourseAccounts { get; } = new List<CourseAccount>();

    public virtual ICollection<Document> Documents { get; } = new List<Document>();

    public virtual ICollection<Class> Classes { get; } = new List<Class>();
}
