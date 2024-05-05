using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class QuizAttempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuizId { get; set; }

    public DateTime CreateDate { get; set; }

    public decimal Score { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
