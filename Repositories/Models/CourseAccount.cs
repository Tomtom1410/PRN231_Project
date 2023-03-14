using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class CourseAccount
{
    public long AccountId { get; set; }

    public long CourseId { get; set; }

    public bool? IsAuthor { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Course Course { get; set; } = null!;
}
