using coursify_backend.DTO.INTERNAL;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.AspNetCore.Hosting;

namespace coursify_backend.Repository
{
    public class SlideRepository(CoursifyContext context) : ISlideRepository
    {
        private readonly CoursifyContext _context = context;

        public async Task<bool> Add(Slide slide)
        {
           _context.Slides.Add(slide);
           return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteCollection(ICollection<Slide> slides)
        {
            _context.Slides.RemoveRange(slides);
            return await _context.SaveChangesAsync() > 0;  
        }
    }
}
