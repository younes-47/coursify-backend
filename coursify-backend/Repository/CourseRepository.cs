using coursify_backend.DTO.GET;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace coursify_backend.Repository
{
    public class CourseRepository(CoursifyContext context, IWebHostEnvironment webHostEnvironment) : ICourseRepository
    {
        private readonly CoursifyContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<bool> Add(Course course)
        {
            await _context.Courses.AddAsync(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<CourseAdminTableDTO>> GetAll()
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Select(c => new CourseAdminTableDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Category = c.Category.Title,
                })
                .ToListAsync();
        }

        public async Task<Course> GetEverythingById(int id)
        {
            return await _context.Courses
                .Include(c => c.Sections)
                .ThenInclude(s => s.Slides)
                .Include(c => c.Sections)
                .ThenInclude(s => s.Documents)
                .Include(c => c.Evaluation)
                .ThenInclude(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .Include(c => c.Evaluation)
                .ThenInclude(e => e.EvaluationAttempts)
                .Include(c => c.Quiz)
                .ThenInclude(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .Include(c => c.Quiz)
                .ThenInclude(q => q.QuizAttempts)
                .Include(c => c.Enrollments)
                .FirstAsync(c => c.Id == id);
        }

        public async Task<bool> IsExisted(int id)
        {
            return await _context.Courses.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Delete(Course course)
        {
            _context.Courses.Remove(course);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> GetTotal()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<List<CourseDetailsDTO>> GetAvailable(string email)
        {
            return await _context.Courses
            .Include(c => c.Category)
            .Select(c => new CourseDetailsDTO
            {
                Id = c.Id,
                Title = c.Title,
                Category = c.Category.Title,
                Cover = c.Cover != "PLACEHOLDER" ? File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\{c.Id}\\{c.Cover}") : null,
                TotalSections = c.Sections.Count,
                TotalSlides = c.Sections.Select(s => s.Slides.Count).Count(),
                TotalDocuments = c.Sections.Select(s => s.Documents.Count).Count(),
                IsEnrolled = c.Enrollments.Any(e => e.User.Email == email)
            })
            .ToListAsync();
        }

        public async Task<CourseDetailsDTO> GetDetailsById(int id, string email)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Select(c => new CourseDetailsDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Category = c.Category.Title,
                    Description = c.Description,
                    Cover = c.Cover != "PLACEHOLDER" ? File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\{c.Id}\\{c.Cover}") : null,
                    TotalSections = c.Sections.Count,
                    TotalSlides = c.Sections.Select(s => s.Slides.Count).Count(),
                    TotalDocuments = c.Sections.Select(s => s.Documents.Count).Count(),
                    IsEnrolled = c.Enrollments.Any(e => e.User.Email == email)
                })
                .FirstAsync(c => c.Id == id);
        }

        public async Task<List<CourseDetailsDTO>> GetEnrolled(string email)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .Where(c => c.Enrollments.Any(e => e.User.Email == email))
                .Select(c => new CourseDetailsDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Category = c.Category.Title,
                    Cover = c.Cover != "PLACEHOLDER" ? File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\{c.Id}\\{c.Cover}") : null,
                    TotalSections = c.Sections.Count,
                    TotalSlides = c.Sections.Select(s => s.Slides.Count).Count(),
                    TotalDocuments = c.Sections.Select(s => s.Documents.Count).Count(),
                    IsEnrolled = true
                })
                .ToListAsync();
        }
       
    }
}
