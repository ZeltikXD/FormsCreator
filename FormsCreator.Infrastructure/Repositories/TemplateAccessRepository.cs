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
    internal class TemplateAccessRepository(FormsDbContext context, ILogger<TemplateAccessRepository> logger)
        : RepositoryBase<TemplateAccess>(context, logger), ITemplateAccessRepository
    {
        public async Task<IResult> CreateAsync(TemplateAccess access)
            => await ExecuteAddAsync(async () =>
            {
                _context.TemplateAccesses.Add(access);
                await _context.SaveChangesAsync();
                return Result.Success(Guid.Empty);
            });

        public async Task<IResult> CreateRangeAsync(IEnumerable<TemplateAccess> access)
                => await ExecuteAddAsync(async () =>
                {
                    var newAccessList = new List<TemplateAccess>();
                    foreach (var a in access)
                    {
                        bool exists = await _context.TemplateAccesses
                            .AnyAsync(ta => ta.UserId == a.UserId && ta.TemplateId == a.TemplateId);
                        if (!exists)
                        {
                            newAccessList.Add(a);
                        }
                    }
                    if (newAccessList.Count != 0)
                    {
                        _context.TemplateAccesses.AddRange(newAccessList);
                        await _context.SaveChangesAsync();
                    }

                    return Result.Success(Guid.Empty);
                });

        public Task<IResult> DeleteAsync(Guid userId, Guid templateId)
            => ExecuteDeleteAsync(async () =>
            {
                var current = await _context.TemplateAccesses.FindAsync(userId, templateId);
                if (current is null) return Result.Failure(new(ResultErrorType.NotFoundError, "The permission you are trying to revoke couldn't be found."));
                _context.TemplateAccesses.Remove(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, InternalMessages.TemplateAcessPermissionNotRevoked));
            });

        public Task<IResult> DeleteRangeAsync(IEnumerable<TemplateAccess> access)
            => ExecuteDeleteAsync(async () =>
            {
                _context.TemplateAccesses.RemoveRange(access);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, InternalMessages.TemplateAcessPermissionNotRevoked));
            });

        public async Task<IResult<bool>> UserHasPermissionAsync(Guid userId, Guid templateId, CancellationToken token = default)
        {
            try
            {
                var res = await _context.TemplateAccesses.AnyAsync(x => x.UserId == userId && x.TemplateId == templateId, token);
                return Result.Success(res);
            }
            catch (Exception ex)
            {
                LogError("An error occurred while comprobating information.", ex);
                return Result.Failure<bool>(new(ResultErrorType.UnknownError, InternalMessages.CountingError));
            }
        }
    }
}
