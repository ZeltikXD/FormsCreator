using EntityFramework.Exceptions.Common;
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
    internal class CommentRepository(FormsDbContext context, ILogger<CommentRepository> logger)
        : RepositoryBase<Comment>(context, logger), ICommentRepository
    {
        public Task<IResult<Guid>> CreateAsync(Comment entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Comments.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult<IEnumerable<Comment>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Comments.Include(x => x.User)
                    .Where(x => x.TemplateId == templateId).OrderBy(x => x.CreatedAt)
                    .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Comments.LongCountAsync(x => x.TemplateId == templateId, token));
    }
}
