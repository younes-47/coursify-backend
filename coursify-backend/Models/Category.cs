using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
