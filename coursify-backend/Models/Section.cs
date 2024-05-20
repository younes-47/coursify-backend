using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Section
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<CourseProgress> CourseProgresses { get; set; } = new List<CourseProgress>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Slide> Slides { get; set; } = new List<Slide>();
}
