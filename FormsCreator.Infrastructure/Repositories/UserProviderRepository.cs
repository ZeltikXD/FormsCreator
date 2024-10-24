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
    internal class UserProviderRepository(FormsDbContext context, ILogger<UserProviderRepository> logger)
        : RepositoryBase<UserProvider>(context, logger), IUserProviderRepository
    {
        public Task<IResult<Guid>> CreateAsync(UserProvider entity)
            => ExecuteAddAsync(async () =>
            {
                _context.Providers.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult> DeleteAsync(Guid id)
            => ExecuteDeleteAsync(async () =>
            {
                var current = await _context.Providers.FindAsync(id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The provider you are trying to delete couldn't be found."));
                _context.Providers.Remove(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The provider couldn't be deleted."));
            });

        public Task<IResult<IEnumerable<UserProvider>>> GetProvidersAsync(Guid userId, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Providers.Where(x => x.UserId == userId).ToListAsync(token));

        public Task<IResult> UpdateAsync(UserProvider provider)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await _context.Providers.FindAsync(provider.Id);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The provider couldn't be found."));
                current.AccessToken = provider.AccessToken;
                current.ExpireTime = provider.ExpireTime;
                _context.Providers.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, "The provider couldn't be updated."));
            });
    }
}
