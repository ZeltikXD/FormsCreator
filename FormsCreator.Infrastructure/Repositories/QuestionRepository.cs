using FormsCreator.Application.Resources;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class QuestionRepository(FormsDbContext context, ILogger<QuestionRepository> logger)
        : RepositoryBase<Question>(context, logger), IQuestionRepository
    {
        public Task<IResult<Guid>> CreateAsync(Question entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Questions.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult> DeleteAsync(Guid id)
            => ExecuteDeleteAsync(async () =>
            {
                var current = await _context.Questions.FindAsync(id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The question you are trying to delete doesn't exist."));
                _context.Questions.Remove(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The question couldn't be deleted."));
            });

        public Task<IResult<IEnumerable<Question>>> GetByTemplateAsync(Guid templateId, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Questions.Where(x => x.TemplateId == templateId)
                .OrderBy(x => x.Index).ToListAsync(token));

        public Task<IResult> UpdateAsync(Question question)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await _context.Questions.FindAsync(question.Id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The question you are trying to update doesn't exist."));
                current.Text = question.Text;
                current.Description = question.Description;
                current.IsVisibleInTable = question.IsVisibleInTable;
                current.Index = question.Index;
                _context.Questions.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The question couldn't be updated."));
            });

        public Task<IResult> UpdateRangeAsync(IEnumerable<Question> questions)
            => ExecuteUpdateAsync(async () =>
            {
                if (!questions.Any())
                    return Result.Failure(new(ResultErrorType.UnprocessableEntityError, "No questions provided."));

                var templateId = questions.First().TemplateId;

                var existingQuestions = await _context.Questions
                                                      .Where(x => x.TemplateId == templateId)
                                                      .ToListAsync();

                var newQuestionIds = questions.Select(q => q.Id).ToHashSet();

                var questionsToDelete = existingQuestions.Where(x => !newQuestionIds.Contains(x.Id)).ToList();
                if (questionsToDelete.Count != 0)
                {
                    _context.Questions.RemoveRange(questionsToDelete);
                }

                foreach (var q in questions)
                {
                    if (q.Id == default)
                    {
                        _context.Questions.Add(q);
                    }
                    else
                    {
                        var current = existingQuestions.FirstOrDefault(x => x.Id == q.Id);
                        if (current is not null)
                        {
                            UpdateQuestion(current, q);
                        }
                    }
                }
                int rows = await _context.SaveChangesAsync();
                var message = GetMessage(rows, questions.Count());
                return GetBoolean(rows, questions.Count()) ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, message));
            });

        void UpdateQuestion(Question target, Question source)
        {
            target.Text = source.Text;
            target.Description = source.Description;
            target.IsVisibleInTable = source.IsVisibleInTable;
            target.Index = source.Index;
            _context.Questions.Update(target);
        }

        static bool GetBoolean(int rows, int totalCount)
        {
            if (rows > 0 && rows < totalCount) return false;

            if (rows > 0 && rows == totalCount) return true;

            return false;
        }

        static string GetMessage(int rows, int totalCount)
        {
            if (rows > 0 && rows < totalCount)
                return InternalMessages.AllQuestionsNotUpdated;

            if (rows > 0 && rows == totalCount) return string.Empty;

            return InternalMessages.SomeQuestionsNotUpdated;
        }
    }
}
