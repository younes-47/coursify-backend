using coursify_backend.DTO.GET;
using coursify_backend.DTO.POST;
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
                .AsSplitQuery()
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
                TotalSlides = c.Sections.SelectMany(s => s.Slides).Count(),
                TotalDocuments = c.Sections.SelectMany(s => s.Documents).Count(),
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
                    TotalSlides = c.Sections.SelectMany(s => s.Slides).Count(),
                    TotalDocuments = c.Sections.SelectMany(s => s.Documents).Count(),
                    IsEnrolled = c.Enrollments.Any(e => e.User.Email == email)
                })
                .FirstAsync(c => c.Id == id);
        }

        public async Task<List<EnrolledCourseDetailsDTO>> GetEnrolled(string email)
        {
            return await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Enrollments)
                .Include(c => c.Quiz)
                .Include(c => c.Sections)
                .ThenInclude(s => s.CourseProgresses)
                .Where(c => c.Enrollments.Any(e => e.User.Email == email))
                .Select(c => new EnrolledCourseDetailsDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Category = c.Category.Title,
                    Cover = c.Cover != "PLACEHOLDER" ? File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\{c.Id}\\{c.Cover}") : null,
                    HighestQuizScore = c.Quiz.QuizAttempts.Where(qa => qa.User.Email == email).Max(qa => qa.Score),
                    IsCompleted = c.Sections.All(s => s.CourseProgresses.Any(cp => cp.User.Email == email && cp.IsCompleted)),
                    Progress = Math.Round(((c.Sections.SelectMany(s => s.CourseProgresses).Count(cp => cp.User.Email == email && cp.IsCompleted) / (decimal)c.Sections.Count) * 100) , 2)
                })
                .ToListAsync();
        }

        public async Task<CourseContentDTO> GetContent(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Sections)
                .ThenInclude(s => s.Slides)
                .Include(c => c.Sections)
                .ThenInclude(s => s.Documents)
                .Include(c => c.Sections)
                .ThenInclude(s => s.CourseProgresses)
                .Select(c => new CourseContentDTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Sections = c.Sections.Select(s => new coursify_backend.DTO.GET.SectionDTO
                    {
                        Id = s.Id,
                        Title = s.Title,
                        IsCompleted = s.CourseProgresses.Any(cp => cp.SectionId == s.Id && cp.IsCompleted),
                        Slides = s.Slides.Select(s => File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\{c.Id}\\{s.SlideName}")).ToList(),
                        Documents = s.Documents.Select(d => File.ReadAllBytes(_webHostEnvironment.WebRootPath + $"\\{c.Id}\\{d.DocumentName}")).ToList()
                    }).ToList()
                })
                .FirstAsync(c => c.Id == courseId);
        }
       
    }
}
