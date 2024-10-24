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
    internal class FormRepository(FormsDbContext context, ILogger<FormRepository> logger)
        : RepositoryBase<Form>(context, logger), IFormRepository
    {
        public Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Forms.LongCountAsync(x => x.TemplateId == templateId, token));

        public Task<IResult<long>> CountByUserAsync(Guid userId, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Forms.LongCountAsync(x => x.UserId == userId, token));

        public Task<IResult<Guid>> CreateAsync(Form entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Forms.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        Task<IResult<Form>> IFormRepository.FindAsync(Guid id, CancellationToken token)
            => ExecuteFindAsync(async () =>
            {
                var result = await FindAsync(id, token);
                if (result is null) return Result.Failure<Form>(new(ResultErrorType.NotFoundError, "The form you requested doesn't exist."));
                return Result.Success(result);
            });

        public async Task<IResult> ExistsAsync(Guid id, CancellationToken token = default)
            => await ExecuteFindAsync(async () =>
            {
                var result = await _context.Forms.AnyAsync(x => x.Id == id, token);
                return result ? Result.Success<Form>(null!)
                : Result.Failure<Form>(new(ResultErrorType.NotFoundError, "The form doesn't exist."));
            });

        public Task<IResult<IEnumerable<Form>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Forms.Include(x => x.User).Include(x => x.Template).Where(x => x.TemplateId == templateId).OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<Form>>> GetByUserAsync(Guid userId, int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Forms.Include(x => x.Template).Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt)
                    .Skip((page - 1) * size).Take(size).ToListAsync(token));

        private Task<Form?> FindAsync(Guid id, CancellationToken token = default)
            => _context.Forms.Include(x => x.Template).Include(x => x.Answers).ThenInclude(x => x.Options).FirstOrDefaultAsync(x => x.Id == id, token);
    }
}
