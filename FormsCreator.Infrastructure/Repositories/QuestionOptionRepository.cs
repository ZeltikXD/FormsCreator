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
    internal class QuestionOptionRepository(FormsDbContext context, ILogger<QuestionOptionRepository> logger)
        : RepositoryBase<QuestionOption>(context, logger), IQuestionOptionRepository
    {
        public Task<IResult<Guid>> CreateAsync(QuestionOption entity)
            => ExecuteAddAsync(async () =>
            {
                _context.QuestionOptions.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult> DeleteAsync(Guid id)
            => ExecuteDeleteAsync(async () =>
            {
                var current = await _context.QuestionOptions.FindAsync(id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The option you are trying to delete doesn't exist."));
                _context.QuestionOptions.Remove(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The option couldn't be deleted."));
            });

        public Task<IResult<IEnumerable<QuestionOption>>> GetByQuestionAsync(Guid questionId, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.QuestionOptions.Where(x => x.QuestionId == questionId).ToListAsync(token));

        public Task<IResult> UpdateAsync(QuestionOption questionOption)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await _context.QuestionOptions.FindAsync(questionOption.Id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The option you are trying to update doesn't exist."));
                current.Value = questionOption.Value;
                current.Row = questionOption.Row;
                current.Column = questionOption.Column;
                _context.QuestionOptions.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success() :
                    Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The option couldn't be updated."));
            });

        public Task<IResult> UpdateRangeAsync(IEnumerable<QuestionOption> options)
            => ExecuteUpdateAsync(async () =>
            {
                var questionIds = options.Select(o => o.QuestionId).Distinct();

                var currentOptions = await _context.QuestionOptions
                    .Where(q => questionIds.Contains(q.QuestionId))
                    .ToListAsync();

                var optionsToKeep = options.Select(o => o.Id).ToList();
                var optionsToRemove = currentOptions
                    .Where(current => !optionsToKeep.Contains(current.Id))
                    .ToList();

                if (optionsToRemove.Count != 0)
                {
                    _context.QuestionOptions.RemoveRange(optionsToRemove);
                }
                foreach (var questionOption in options)
                {
                    if (questionOption.Id == default)
                    {
                        _context.QuestionOptions.Add(questionOption);
                        continue;
                    }

                    var current = currentOptions.FirstOrDefault(x => x.Id == questionOption.Id && x.QuestionId == questionOption.QuestionId);
                    if (current is null) continue;
                    UpdateOption(questionOption, current);
                }
                int rows = await _context.SaveChangesAsync();
                var message = GetMessage(rows, options.Count());

                return GetBoolean(rows, options.Count()) ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, message));
            });

        void UpdateOption(QuestionOption src, QuestionOption target)
        {
            target.Value = src.Value;
            target.Row = src.Row;
            target.Column = src.Column;
            _context.QuestionOptions.Update(target);
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
