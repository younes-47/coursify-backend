using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;

namespace coursify_backend.Repository
{
    public class DocumentRepository(CoursifyContext coursifyContext) : IDocumentRepository
    {
        private readonly CoursifyContext _context = coursifyContext;

        public async Task<bool> Add(Document document)
        {
           await _context.Documents.AddAsync(document);
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCollection(ICollection<Document> documents)
        {
            _context.Documents.RemoveRange(documents);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
