namespace coursify_backend.Interfaces.IService
{
    public interface ISectionService
    {
        Task<bool> MarkCompleted(string email, int sectionId);
        Task<bool> MarkIncomplete(string email, int sectionId);
    }
}
