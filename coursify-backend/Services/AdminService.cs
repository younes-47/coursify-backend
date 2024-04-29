using AutoMapper;
using coursify_backend.DTO.INTERNAL;
using coursify_backend.DTO.POST;
using coursify_backend.Interfaces.IRepository;
using coursify_backend.Interfaces.IService;
using coursify_backend.Models;
using coursify_backend.Repository;

namespace coursify_backend.Services
{
    public class AdminService(IMapper mapper, 
        CoursifyContext context,
        ICourseRepository courseRepository,
        ISectionRepository sectionRepository,
        ISlideRepository slideRepository,
        IDocumentRepository documentRepository,
        IEvaluationRepository evaluationRepository,
        IQuestionRepository questionRepository,
        IAnswerRepository answerRepository,
        IQuizRepository quizRepository,
        IMiscService miscService) : IAdminService
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
        private readonly IMiscService _miscService = miscService;

        public async Task<ProcessResult> AddCourse(AddCourseDTO addCourseDTO)
        {
            ProcessResult processResult = new();
            var transaction = _context.Database.BeginTransaction();
            try
            {
                // BASIC COURSE INFO
                Course course = new()
                {
                    Title = addCourseDTO.CourseInfo.CourseTitle,
                    Description = addCourseDTO.CourseInfo.CourseDescription,
                    CategoryId = addCourseDTO.CourseInfo.CourseCategoryId
                };
                await _courseRepository.Add(course);

                List<FileDetails> FilesToWrite = [];

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

                        FileDetails fileDetails = _miscService.GetFileDetails(slide, ".png", course.Id);
                        FilesToWrite.Add(fileDetails);

                        newSlide.SlidePath = fileDetails.FilePath;
                        
                        await _slideRepository.Add(newSlide);
                    }

                    // DOCUMENTS
                    foreach (Byte[] doc in section.Documents)
                    {
                        Document newDoc = new();
                        newDoc.SectionId = newSection.Id;

                        FileDetails fileDetails = _miscService.GetFileDetails(doc, ".pdf" , course.Id);
                        FilesToWrite.Add(fileDetails);

                        newDoc.DocumentPath = fileDetails.FilePath;

                        await _documentRepository.Add(newDoc);
                    }
                }

                // EVALUATION QUESTIONS
                foreach (EvaluationQuestionsDTO question in addCourseDTO.EvaluationQuestions)
                {
                    Evaluation evaluation = new();
                    evaluation.CourseId = course.Id;

                    await _evaluationRepository.Add(evaluation);

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
                foreach (QuizQuestionsDTO question in addCourseDTO.QuizQuestions)
                {
                    Quiz quiz = new();
                    quiz.CourseId = course.Id;

                    await _quizRepository.Add(quiz);

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
                foreach (FileDetails file in FilesToWrite)
                {
                    await System.IO.File.WriteAllBytesAsync(file.FilePath, file.FileData);
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



    }
}
