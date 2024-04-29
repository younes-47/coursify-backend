using coursify_backend.DTO.INTERNAL;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.AspNetCore.Hosting;

namespace coursify_backend.Repository
{
    public class SlideRepository(CoursifyContext context,
        IWebHostEnvironment webHostEnvironment) : ISlideRepository
    {
        private readonly CoursifyContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<bool> Add(Slide slide)
        {
           _context.Slides.Add(slide);
           return await _context.SaveChangesAsync() > 0;
        }

        
    }
}
