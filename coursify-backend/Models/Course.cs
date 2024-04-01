using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Evaluation Evaluation { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual List<Section> Sections { get; set; } = new List<Section>();
}
