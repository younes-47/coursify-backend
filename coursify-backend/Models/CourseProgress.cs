namespace coursify_backend.Models
{
    public class CourseProgress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SectionId { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Section Section { get; set; } = null!;
    }
}
