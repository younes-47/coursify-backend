using coursify_backend.Models;

namespace coursify_backend.Interfaces.IRepository
{
    public interface IDocumentRepository
    {
        Task<bool> Add(Document document);
        Task<bool> DeleteCollection(ICollection<Document> documents);
    }
}
