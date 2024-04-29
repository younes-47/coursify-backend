using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Question
{
    public int Id { get; set; }

    public int? EvaluationId { get; set; }

    public int? QuizId { get; set; }

    public string QuestionText { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Evaluation? Evaluation { get; set; }

    public virtual Quiz? Quiz { get; set; }
}
