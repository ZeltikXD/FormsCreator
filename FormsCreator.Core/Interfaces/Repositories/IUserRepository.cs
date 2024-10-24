using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<IResult<IEnumerable<User>>> GetUnblockedAsync(int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<User>>> GetBlockedAsync(int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<User>>> GetAllAsync(int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<User>>> SearchBySimilarityAsync(string text, CancellationToken token = default);

        Task<IResult<User>> FindByNameAsync(string userName, CancellationToken token = default);

        Task<IResult<User>> FindByIdAsync(Guid id, CancellationToken token = default);

        Task<IResult<User>> FindByEmailAsync(string email, CancellationToken token = default);

        /// <summary>
        /// Verifys the user password.
        /// </summary>
        /// <param name="nameOrEmail">Sets the username or email.</param>
        /// <param name="password">The attempted password.</param>
        /// <param name="isEmail">If <see langword="true"/>, <paramref name="nameOrEmail"/> is email; otherwise is username.</param>
        /// <returns>An operation result.</returns>
        Task<IResult> IsPassCorrectAsync(string nameOrEmail, string password, bool isEmail, CancellationToken token = default);

        Task<IResult> ChangeRoleAsync(Guid id, Guid roleId);

        Task<IResult> ChangeStatusAsync(Guid id, bool isBlocked);

        Task<IResult<long>> CountAllAsync(CancellationToken token = default);

        Task<IResult<long>> CountBlockedAsync(CancellationToken token = default);

        Task<IResult<long>> CountUnblockedAsync(CancellationToken token = default);

        Task<IResult> DeleteAsync(Guid id);

        Task<IResult<bool>> HasProvidersAsync(string nameOrEmail, CancellationToken token = default);
    }
}
