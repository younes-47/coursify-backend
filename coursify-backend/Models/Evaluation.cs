﻿using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class Evaluation
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual List<EvaluationAttempt> EvaluationAttempts { get; set; } = new List<EvaluationAttempt>();

    public virtual List<Question> Questions { get; set; } = new List<Question>();
}