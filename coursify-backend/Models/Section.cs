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

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual List<Document> Documents { get; set; } = new List<Document>();

    public virtual List<Slide> Slides { get; set; } = new List<Slide>();

    public virtual List<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
