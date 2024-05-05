using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class EvaluationAttempt
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int EvaluationId { get; set; }

    public decimal Score { get; set; }

    public bool IsPassed { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Evaluation Evaluation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
