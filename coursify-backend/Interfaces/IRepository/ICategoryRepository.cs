using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
    }
}
