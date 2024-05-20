using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ISectionRepository
    {
        Task<bool> Add(Section section);
        Task<bool> IsExisted(int id);
        Task<bool> DeleteCollection(ICollection<Section> sections);
        Task<List<Section>> GetByCourseId(int courseId);
    }
}
