﻿using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class QuizAttempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CourseId { get; set; }

    public DateTime CreateDate { get; set; }

    public decimal Score { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
