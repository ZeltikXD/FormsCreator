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
    internal class TagRepository(FormsDbContext context, ILogger<TagRepository> logger) 
        : RepositoryBase<Tag>(context, logger), ITagRepository
    {
        public Task<IResult<Guid>> CreateAsync(Tag entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Tags.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        Task<IResult<Tag>> ITagRepository.FindAsync(string tagName, CancellationToken token)
            => ExecuteFindAsync(async () =>
            {
                var res = await _context.Tags.FirstOrDefaultAsync(x => x.Name.ToLower() == tagName.ToLower());
                if (res is null) return Result.Failure<Tag>(new(ResultErrorType.NotFoundError, string.Empty));
                return Result.Success(res);
            });

        public Task<IResult<Tag>> FindAsync(Guid id, CancellationToken token = default)
            => ExecuteFindAsync(async () =>
            {
                var res = await _context.Tags.FindAsync([id], token);
                if (res is null) return Result.Failure<Tag>(new(ResultErrorType.NotFoundError, string.Empty));
                return Result.Success(res);
            });

        public Task<IResult<IEnumerable<Tag>>> GetAsync(CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Tags.ToListAsync(token));

        public Task<IResult<IEnumerable<Tag>>> GetAsync(int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Tags.OrderBy(x => x.Templates.LongCount())
                .Take(size).ToListAsync(token));
    }
}
