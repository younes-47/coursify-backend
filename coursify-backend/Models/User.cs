using System;
using System.Collections.Generic;

namespace coursify_backend.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Role { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? EmailVerificationToken { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public string? PasswordResetToken { get; set; }

    public string? RefreshToken { get; set; }

    public string Avatar { get; set; } = null!;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    public virtual ICollection<EvaluationAttempt> EvaluationAttempts { get; set; } = new List<EvaluationAttempt>();

    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
