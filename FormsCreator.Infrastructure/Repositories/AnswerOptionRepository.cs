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
    internal class AnswerOptionRepository(FormsDbContext context, ILogger<AnswerOptionRepository> logger)
        : RepositoryBase<AnswerOption>(context, logger), IAnswerOptionRepository
    {
        public Task<IResult<IEnumerable<AnswerOption>>> GetByAnswerAsync(Guid answerId, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.AnswerOptions.Where(x => x.AnswerId == answerId).ToListAsync(token));

        public Task<IResult> UpdateRangeAsync(IEnumerable<AnswerOption> answerOptions)
         => ExecuteUpdateAsync(async () =>
         {
             var idsToUpdate = answerOptions.Where(opt => opt.Id != default).Select(x => x.Id);
             if (!idsToUpdate.Any()) return Result.Failure(new(ResultErrorType.UnprocessableEntityError, string.Empty));

             var currentOptions = await _context.AnswerOptions.Where(x => x.AnswerId == answerOptions.First().AnswerId && idsToUpdate.Contains(x.Id))
             .ToListAsync();
             foreach (var current in currentOptions)
             {
                 var opt = answerOptions.First(x => x.Id == current.Id);
                 UpdateOption(opt, current);
             }
             int rows = await _context.SaveChangesAsync();
             var message = GetMessage(rows, answerOptions.Count());
             return GetBoolean(rows, answerOptions.Count()) ? Result.Success()
             : Result.Failure(new(ResultErrorType.UnprocessableEntityError, message));
         });

        void UpdateOption(AnswerOption src, AnswerOption target)
        {
            target.QuestionOptionId = src.QuestionOptionId;
            target.Value = src.Value;
            target.Row = src.Row;
            target.Column = src.Column;
            _context.AnswerOptions.Update(target);
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
