using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Avatar { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? PasswordResetToken { get; set; }

    public string? EmailVerificationToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual List<EvaluationAttempt> EvaluationAttempts { get; set; } = new List<EvaluationAttempt>();

    public virtual List<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
