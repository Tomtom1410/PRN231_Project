using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Class
{
    public long Id { get; set; }

    public string? ClassCode { get; set; }

    public string? ClassName { get; set; }

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();

    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}
