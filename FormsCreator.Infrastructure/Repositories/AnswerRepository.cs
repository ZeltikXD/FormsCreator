using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class AnswerRepository(FormsDbContext context, ILogger<AnswerRepository> logger)
        : RepositoryBase<Answer>(context, logger), IAnswerRepository
    {
        public Task<IResult<Guid>> CreateAsync(Answer entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Answers.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult<IEnumerable<Answer>>> GetByFormAsync(Guid formId, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Answers.Include(x => x.Options).Where(x => x.FormId == formId).ToListAsync(token));

        public async Task<bool> AreFromFormAsync(Guid formId, IEnumerable<Guid> answersId, CancellationToken token = default)
        {
            try
            {
                return await _context.Answers.AllAsync(x => x.FormId == formId && answersId.Contains(x.Id), token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An internal error occurred while comprobating answers with forms.");
                return false;
            }
        }
    }
}
