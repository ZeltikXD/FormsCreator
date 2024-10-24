using FormsCreator.Application.Resources;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Repositories.Base;
using FormsCreator.Infrastructure.Utils;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class UserRepository(FormsDbContext context, ILogger<UserRepository> logger)
        : RepositoryBase<User>(context, logger), IUserRepository
    {
        private static readonly IResult<User> _notFound = Result.Failure<User>(new(ResultErrorType.NotFoundError, InternalMessages.UserNotFound));
        private static readonly IResult<User> _couldntBeUpdated = Result.Failure<User>(new(ResultErrorType.UnprocessableEntityError, InternalMessages.UserNotUpdated));

        public Task<IResult> ChangeRoleAsync(Guid id, Guid roleId)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await _context.Users.FindAsync(id);
                if (current is null) return _notFound;
                current.RoleId = roleId;
                _context.Users.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success() : _couldntBeUpdated;
            });

        public Task<IResult> ChangeStatusAsync(Guid id, bool isBlocked)
            => ExecuteUpdateAsync(async () =>
            {
                var current = await _context.Users.FindAsync(id);
                if (current is null) return _notFound;
                if (current.IsBlocked == isBlocked) return Result.Success();
                current.IsBlocked = isBlocked;
                _context.Users.Update(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success() : _couldntBeUpdated;
            });

        public Task<IResult<long>> CountAllAsync(CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Users.LongCountAsync(token));

        public Task<IResult<long>> CountBlockedAsync(CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Users.LongCountAsync(x => x.IsBlocked, token));

        public Task<IResult<long>> CountUnblockedAsync(CancellationToken token = default)
            => ExecuteCountAsync(() => _context.Users.LongCountAsync(x => !x.IsBlocked, token));

        public Task<IResult<Guid>> CreateAsync(User entity)
            => ExecuteAddAsync(async () =>
            {
                var (password, salt) = HashUtils.HashPassword(entity.PasswordHash);
                entity.PasswordHash = password;
                entity.PasswordSalt = salt;
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                return Result.Success(entity.Id);
            });

        public Task<IResult<User>> FindByEmailAsync(string email, CancellationToken token = default)
            => ExecuteFindAsync(async () => 
            {
                var current = await BaseQuery().FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower(), token);
                if (current is null) return _notFound;
                return Result.Success(current);
            });

        public Task<IResult<User>> FindByIdAsync(Guid id, CancellationToken token = default)
            => ExecuteFindAsync(async () =>
            {
                var current = await BaseQuery().FirstOrDefaultAsync(x => x.Id == id, token);
                if (current is null) return _notFound;
                return Result.Success(current);
            });

        public Task<IResult<User>> FindByNameAsync(string userName, CancellationToken token = default)
            => ExecuteFindAsync(async () =>
            {
                var current = await BaseQuery().FirstOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower(), token);
                if (current is null) return _notFound;
                return Result.Success(current);
            });

        public Task<IResult<IEnumerable<User>>> GetAllAsync(int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<User>>> GetBlockedAsync(int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().Where(x => x.IsBlocked)
            .OrderByDescending(x => x.CreatedAt).Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<User>>> GetUnblockedAsync(int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await BaseQuery().Where(x => !x.IsBlocked)
            .OrderByDescending(x => x.CreatedAt).Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<User>>> SearchBySimilarityAsync(string text, CancellationToken token = default)
            => ExecuteGetAsync(async () =>
            {
                const string query = "SELECT * FROM \"Users\" WHERE \"UserName\" ILIKE @text";

                return await _context.Users.FromSqlRaw(query, new NpgsqlParameter("@text", $"%{text}%")).ToListAsync(token);
            });

        public async Task<IResult> IsPassCorrectAsync(string nameOrEmail, string password, bool isEmail, CancellationToken token = default)
            => await ExecuteFindAsync(async () =>
            {
                var current = await (isEmail ? _context.Users.FirstOrDefaultAsync(x => x.Email == nameOrEmail, token)
                    : _context.Users.FirstOrDefaultAsync(x => x.UserName == nameOrEmail, token));
                if (current is null) return _notFound;
                var isCorrect = HashUtils.CheckHash(password, current.PasswordHash, current.PasswordSalt);
                return isCorrect ? Result.Success<User>(null!)
                    : Result.Failure<User>(new(ResultErrorType.ValidationError, InternalMessages.IncorrectPassword));
            });

        public Task<IResult> DeleteAsync(Guid id)
            => ExecuteDeleteAsync(async () =>
            {
                var current = await _context.Users.FindAsync(id);
                if (current is null) return _notFound;
                _context.Users.Remove(current);
                int rows = await _context.SaveChangesAsync();
                return rows > 0 ? Result.Success()
                    : Result.Failure(new(ResultErrorType.UnprocessableEntityError, InternalMessages.UserNotDeleted));
            });

        public async Task<IResult<bool>> HasProvidersAsync(string nameOrEmail, CancellationToken token = default)
        {
            try
            {
                var res = await _context.Users.AnyAsync(x => (x.UserName.ToLower() == nameOrEmail.ToLower() || x.Email.ToLower() == nameOrEmail.ToLower()) 
                    && x.Providers.LongCount() > 0, token);
                return Result.Success(res);
            }
            catch (Exception ex)
            {
                LogError("An exception has occurred while comprobating user providers.", ex);
                return Result.Failure<bool>(new(ResultErrorType.UnknownError, InternalMessages.CountingError));
            }
        }

        IQueryable<User> BaseQuery()
            => _context.Users.Include(x => x.Role);
    }
}
