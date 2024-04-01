using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Document
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public string DocumentPath { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual Section Section { get; set; } = null!;
}
