using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Quiz
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual List<Answer> Answers { get; set; } = new List<Answer>();

    public virtual List<Question> Questions { get; set; } = new List<Question>();

    public virtual Section Section { get; set; } = null!;
}
