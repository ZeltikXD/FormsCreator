using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IResult> RegisterAsync(UserRegisterRequestDto req);

        Task<IResult<IEnumerable<UserPublicResponseDto>>> GetAllAsync(int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<UserPublicResponseDto>>> GetBlockedAsync(int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<UserPublicResponseDto>>> GetUnblockedAsync(int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<UserPublicResponseDto>>> SearchBySimilarityAsync(string text, CancellationToken token);

        Task<IResult> ChangeStatusAsync(Guid id, bool isBlocked);

        Task<IResult> ChangeRoleAsync(Guid id, Guid roleId);

        Task<IResult<long>> CountAllAsync(CancellationToken token);

        Task<IResult<long>> CountBlockedAsync(CancellationToken token);

        Task<IResult<long>> CountUnblockedAsync(CancellationToken token);

        Task<IResult> DeleteAsync(Guid id);
    }
}
