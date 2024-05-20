using System;
using System.Collections.Generic;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend;

public partial class CoursifyContext : DbContext
{
    public CoursifyContext()
    {
    }

    public CoursifyContext(DbContextOptions<CoursifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseProgress> CourseProgresses { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<EvaluationAttempt> EvaluationAttempts { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<Section> Sections { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Answer__3214EC07CE8CFCC1");

            entity.ToTable("Answer");

            entity.Property(e => e.AnswerText).HasMaxLength(255);
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QUESTION_ANSWER");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07A89089A6");

            entity.ToTable("Category");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course__3214EC07EB3F30B2");

            entity.ToTable("Course");

            entity.HasIndex(e => e.CategoryId, "IX_Course_CategoryId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Cover)
                .HasDefaultValue("PLACEHOLDER")
                .HasMaxLength(255)
                .HasColumnName("Cover");

            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Courses).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<CourseProgress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CoursePr__3214EC07D3A3A3A3");

            entity.ToTable("CourseProgress");

            entity.HasIndex(e => e.SectionId, "IX_CourseProgress_SectionId");

            entity.HasIndex(e => e.UserId, "IX_CourseProgress_UserId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Section).WithMany(p => p.CourseProgresses)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SECTION_COURSE_PROGRESS");

            entity.HasOne(d => d.User).WithMany(p => p.CourseProgresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_COURSE_PROGRESS");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Document__3214EC0738060B46");

            entity.ToTable("Document");

            entity.HasIndex(e => e.SectionId, "IX_Document_SectionId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DocumentName).HasMaxLength(255);

            entity.HasOne(d => d.Section).WithMany(p => p.Documents)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SECTION_DOCUMENT");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Enrollme__3214EC074CB7170A");

            entity.ToTable("Enrollment");

            entity.HasIndex(e => e.CourseId, "IX_Enrollment_CourseId");

            entity.HasIndex(e => e.UserId, "IX_Enrollment_UserId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COURSE_ENROLLMENT");

            entity.HasOne(d => d.User).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ENROLLMENT");
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Evaluati__3214EC070AB12E65");

            entity.ToTable("Evaluation");

            entity.HasIndex(e => e.CourseId, "IX_Evaluation_CourseId").IsUnique();

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithOne(p => p.Evaluation)
                .HasForeignKey<Evaluation>(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COURSE_EVALUATION");
        });

        modelBuilder.Entity<EvaluationAttempt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Evaluati__3214EC074BDEBF90");

            entity.ToTable("EvaluationAttempt");

            entity.HasIndex(e => e.EvaluationId, "IX_EvaluationAttempt_EvaluationId");

            entity.HasIndex(e => e.UserId, "IX_EvaluationAttempt_UserId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.EvaluationAttempts)
                .HasForeignKey(d => d.EvaluationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EVALUATION_EVAL_ATTEMPT");

            entity.HasOne(d => d.User).WithMany(p => p.EvaluationAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_EVAL_ATTEMPT");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07CE35E7EE");

            entity.ToTable("Question");

            entity.HasIndex(e => e.EvaluationId, "IX_Question_EvaluationId");

            entity.HasIndex(e => e.QuizId, "IX_Question_QuizId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.QuestionText)
                .HasMaxLength(255)
                .HasColumnName("Question");

            entity.HasOne(d => d.Evaluation).WithMany(p => p.Questions)
                .HasForeignKey(d => d.EvaluationId)
                .HasConstraintName("FK_QUESTION_EVALUATION");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuizId)
                .HasConstraintName("FK_QUESTION_QUIZ");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quiz__3214EC0791596F0C");

            entity.ToTable("Quiz");

            entity.HasIndex(e => e.CourseId, "IX_Quiz_CourseId").IsUnique();

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Course).WithOne(p => p.Quiz)
                .HasForeignKey<Quiz>(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SECTION_QUIZ");
        });

        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__QuizAtte__3214EC071CEC27EC");

            entity.ToTable("QuizAttempt");

            entity.HasIndex(e => e.QuizId, "IX_QuizAttempt_QuizId");

            entity.HasIndex(e => e.UserId, "IX_QuizAttempt_UserId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizAttempts)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QUIZ_QUIZ_ATTEMPT");

            entity.HasOne(d => d.User).WithMany(p => p.QuizAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_QUIZ_ATTEMPT");
        });

        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Section__3214EC079BF0FE09");

            entity.ToTable("Section");

            entity.HasIndex(e => e.CourseId, "IX_Section_CourseId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Course).WithMany(p => p.Sections)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COURSE_SECTION");
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Slide__3214EC07470D4E06");

            entity.ToTable("Slide");

            entity.HasIndex(e => e.SectionId, "IX_Slide_SectionId");

            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SlideName).HasMaxLength(255);

            entity.HasOne(d => d.Section).WithMany(p => p.Slides)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SECTION_SLIDE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0795AA5E8D");

            entity.ToTable("User");

            entity.Property(e => e.Avatar)
                .HasMaxLength(10)
                .HasDefaultValue("AVATAR0");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EmailVerificationToken).HasMaxLength(255);
            entity.Property(e => e.EmailVerifiedAt).HasColumnType("datetime");
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PasswordResetToken).HasMaxLength(255);
            entity.Property(e => e.RefreshToken).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
