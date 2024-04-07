namespace coursify_backend.DTO.INTERNAL
{
    public class ProcessResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = null!;
        public int? Id { get; set; }
    }
}
