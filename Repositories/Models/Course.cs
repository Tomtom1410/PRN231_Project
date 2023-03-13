using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Course
{
    public long Id { get; set; }

    public string? CourseCode { get; set; }

    public string? CourseName { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<Class> Classes { get; } = new List<Class>();
}
