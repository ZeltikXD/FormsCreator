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
    internal class LikeRepository(FormsDbContext context, ILogger<LikeRepository> logger)
        : RepositoryBase<Like>(context, logger), ILikeRepository
    {
        public async Task<IResult> CreateAsync(Like like)
            => await ExecuteAddAsync(async () =>
            {
                _context.Likes.Add(like);
                await _context.SaveChangesAsync();
                return Result.Success(Guid.Empty);
            });

        public Task<IResult> UpdateAsync(Like like)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await _context.Likes.FindAsync(like.TemplateId, like.UserId);
                if (current is null) return await CreateAsync(like);
                if (current.IsDeleted == like.IsDeleted) return Result.Success();
                current.IsDeleted = like.IsDeleted;
                _context.Likes.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success() :
                    Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The like state couldn't be updated."));
            });

        public Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Likes.Where(x => x.TemplateId == templateId).LongCountAsync(token));
    }
}
