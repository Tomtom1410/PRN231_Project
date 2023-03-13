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

    public virtual ICollection<Account> Accounts { get; } = new List<Account>();
}
