using System;
using System.Collections.Generic;

namespace Repositories.Models;

public partial class Document
{
    public long Id { get; set; }

    public string? DocumentName { get; set; }

    public string? DocumentOriginalName { get; set; }

    public string? ContentType { get; set; }

    public string? PathFile { get; set; }

    public long? AccountId { get; set; }

    public long? CourseId { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Course? Course { get; set; }
}
