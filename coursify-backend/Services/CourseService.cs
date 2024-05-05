using AutoMapper;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using coursify_backend.Repository;
using System.Drawing;

namespace coursify_backend.Services
{
    public class CourseService(IMapper mapper, 
        CoursifyContext context,
        ICourseRepository courseRepository,
        ISectionRepository sectionRepository,
        ISlideRepository slideRepository,
        IDocumentRepository documentRepository,
        IEvaluationRepository evaluationRepository,
        IQuestionRepository questionRepository,
        IAnswerRepository answerRepository,
        IQuizRepository quizRepository,
        IQuizAttempRepository quizAttempRepository,
        IEvaluationAttemptRepository evaluationAttemptRepository,
        IEnrollementRepository enrollementRepository,
        IWebHostEnvironment webHostEnvironment,
        IMiscService miscService) : ICourseService
    {
        private readonly IMapper _mapper = mapper;
        private readonly CoursifyContext _context = context;
        private readonly ICourseRepository _courseRepository = courseRepository;
        private readonly ISectionRepository _sectionRepository = sectionRepository;
        private readonly ISlideRepository _slideRepository = slideRepository;
        private readonly IDocumentRepository _documentRepository = documentRepository;
        private readonly IEvaluationRepository _evaluationRepository = evaluationRepository;
        private readonly IQuestionRepository _questionRepository = questionRepository;
        private readonly IAnswerRepository _answerRepository = answerRepository;
        private readonly IQuizRepository _quizRepository = quizRepository;
        private readonly IQuizAttempRepository _quizAttempRepository = quizAttempRepository;
        private readonly IEvaluationAttemptRepository _evaluationAttemptRepository = evaluationAttemptRepository;
        private readonly IEnrollementRepository _enrollementRepository = enrollementRepository;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IMiscService _miscService = miscService;

        public async Task<ProcessResult> AddCourse(AddCourseDTO addCourseDTO)
        {
            ProcessResult processResult = new();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                List<FileDetails> FilesToSave = [];

                // BASIC COURSE INFO
                Course course = new()
                {
                    Title = addCourseDTO.CourseInfo.CourseTitle,
                    Description = addCourseDTO.CourseInfo.CourseDescription,
                    CategoryId = addCourseDTO.CourseInfo.CourseCategoryId
                };
                if(addCourseDTO.CourseInfo.CourseCover != null)
                {
                    FileDetails fileDetails = new()
                    {
                        FileData = addCourseDTO.CourseInfo.CourseCover,
                        FileName = Guid.NewGuid().ToString() + ".png"
                    };
                    FilesToSave.Add(fileDetails);
                    course.Cover = fileDetails.FileName;
                }
                await _courseRepository.Add(course);

                

                // SECTIONS
                foreach (var section in addCourseDTO.Sections)
                {
                    Section newSection = new()
                    {
                        Title = section.Title,
                        CourseId = course.Id
                    };
                    await _sectionRepository.Add(newSection);

                    // SLIDES
                    foreach (Byte[] slide in section.Slides)
                    {
                        Slide newSlide = new();
                        newSlide.SectionId = newSection.Id;

                        FileDetails fileDetails = new()
                        {
                            FileData = slide,
                            FileName = Guid.NewGuid().ToString() + ".png"
                        }; 
                        FilesToSave.Add(fileDetails);

                        newSlide.SlideName = fileDetails.FileName;
                        
                        await _slideRepository.Add(newSlide);
                    }

                    // DOCUMENTS
                    foreach (Byte[] doc in section.Documents)
                    {
                        Document newDoc = new();
                        newDoc.SectionId = newSection.Id;

                        FileDetails fileDetails = new()
                        {
                            FileData = doc,
                            FileName = Guid.NewGuid().ToString() + ".pdf"
                        };
                        FilesToSave.Add(fileDetails);
                        newDoc.DocumentName = fileDetails.FileName;

                        await _documentRepository.Add(newDoc);
                    }
                }

                // EVALUATION QUESTIONS
                Evaluation evaluation = new();
                evaluation.CourseId = course.Id;
                await _evaluationRepository.Add(evaluation);
                foreach (EvaluationQuestionsDTO question in addCourseDTO.EvaluationQuestions)
                {
                    Question newQuestion = new();
                    newQuestion.EvaluationId = evaluation.Id;
                    newQuestion.QuestionText = question.Question;

                    await _questionRepository.Add(newQuestion);

                    // ANSWERS
                    foreach (AnswerDTO answer in question.Answers)
                    {
                        Answer newAnswer = new();
                        newAnswer.QuestionId = newQuestion.Id;
                        newAnswer.AnswerText = answer.Answer;
                        newAnswer.IsCorrect = answer.IsCorrect;

                        await _answerRepository.Add(newAnswer);
                    }
                }
                

                // QUIZ QUESTIONS
                Quiz quiz = new();
                quiz.CourseId = course.Id;
                await _quizRepository.Add(quiz);
                foreach (QuizQuestionsDTO question in addCourseDTO.QuizQuestions)
                {
                    Question newQuestion = new();
                    newQuestion.QuizId = quiz.Id;
                    newQuestion.QuestionText = question.Question;

                    await _questionRepository.Add(newQuestion);

                    // ANSWERS
                    foreach (AnswerDTO answer in question.Answers)
                    {
                        Answer newAnswer = new();
                        newAnswer.QuestionId = newQuestion.Id;
                        newAnswer.AnswerText = answer.Answer;
                        newAnswer.IsCorrect = answer.IsCorrect;

                        await _answerRepository.Add(newAnswer);
                    }
                }
                

                await transaction.CommitAsync();
                processResult.Success = true;

                // WRITE FILES TO DISK AFTER COMMIT TO DB TO AVOID INCONSISTENCY BETWEEN DB AND FILE SYSTEM
                var dirPath = Path.Combine(_webHostEnvironment.WebRootPath, course.Id.ToString());
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                foreach (FileDetails file in FilesToSave)
                {
                    await System.IO.File.WriteAllBytesAsync(dirPath + $"\\{file.FileName}", file.FileData);
                }

            }
            catch (Exception e)
            {
                transaction.Rollback();
                processResult.Success = false;
                processResult.Message = e.Message;
            }

            return processResult;
        }

        public async Task<ProcessResult> DeleteCourse(int courseId)
        {
            ProcessResult result = new();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                Course course = await _courseRepository.GetEverythingById(courseId);

                // DELETE QUIZ AND RELATED DATA
                if (course.Quiz != null)
                {
                    await _quizAttempRepository.DeleteCollection(course.Quiz.QuizAttempts);

                    foreach (Question question in course.Quiz.Questions)
                    {
                        await _answerRepository.DeleteCollection(question.Answers);
                    }
                    await _questionRepository.DeleteCollection(course.Quiz.Questions);
                    await _quizRepository.Delete(course.Quiz);
                }

                // DELETE EVALUATION AND RELATED DATA
                if (course.Evaluation != null)
                {
                    await _evaluationAttemptRepository.DeleteCollection(course.Evaluation.EvaluationAttempts);
                    foreach (Question question in course.Evaluation.Questions)
                    {
                        await _answerRepository.DeleteCollection(question.Answers);
                    }
                    await _questionRepository.DeleteCollection(course.Evaluation.Questions);
                    await _evaluationRepository.Delete(course.Evaluation);
                }

                // DELETE SECTIONS AND RELATED DATA
                foreach (Section section in course.Sections)
                {
                    await _slideRepository.DeleteCollection(section.Slides);
                    await _documentRepository.DeleteCollection(section.Documents);
                }              
                await _sectionRepository.DeleteCollection(course.Sections);

                // DELETE ENROLLMENTS
                await _enrollementRepository.DeleteCollection(course.Enrollments);

                // DELETE COURSE
                await _courseRepository.Delete(course);

                await transaction.CommitAsync();

                // DELETE FILES AFTER COMMIT TO DB TO AVOID INCONSISTENCY BETWEEN DB AND FILE SYSTEM
                _miscService.DeleteWholeFolder(course.Id);

                result.Success = true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                result.Success = false;
                result.Message = e.Message;
            }

            return result;

        }


    }
}
