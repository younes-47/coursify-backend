using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Slide
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public string SlideName { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual Section Section { get; set; } = null!;
}
