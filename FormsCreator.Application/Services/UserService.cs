using AutoMapper;
using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class UserService(IUserRepository userRepository,
        IMapper mapper) : IUserService
    {
        readonly IUserRepository _userRepository = userRepository;
        readonly IMapper _mapper = mapper;

        public Task<IResult> ChangeStatusAsync(Guid id, bool isBlocked)
        {
            return _userRepository.ChangeStatusAsync(id, isBlocked);
        }

        public Task<IResult> ChangeRoleAsync(Guid id, Guid roleId)
        {
            return _userRepository.ChangeRoleAsync(id, roleId);
        }

        public Task<IResult<long>> CountAllAsync(CancellationToken token)
        {
            return _userRepository.CountAllAsync(token);
        }

        public Task<IResult<long>> CountBlockedAsync(CancellationToken token)
        {
            return _userRepository.CountBlockedAsync(token);
        }

        public Task<IResult<long>> CountUnblockedAsync(CancellationToken token)
        {
            return _userRepository.CountUnblockedAsync(token);
        }

        public async Task<IResult<IEnumerable<UserPublicResponseDto>>> GetAllAsync(int page, int size, CancellationToken token)
        {
            var usersRes = await _userRepository.GetAllAsync(page, size, token);
            return MapToResponse(usersRes);
        }

        public async Task<IResult<IEnumerable<UserPublicResponseDto>>> GetBlockedAsync(int page, int size, CancellationToken token)
        {
            var usersRes = await _userRepository.GetBlockedAsync(page, size, token);
            return MapToResponse(usersRes);
        }

        public async Task<IResult<IEnumerable<UserPublicResponseDto>>> GetUnblockedAsync(int page, int size, CancellationToken token)
        {
            var usersRes = await _userRepository.GetUnblockedAsync(page, size, token);
            return MapToResponse(usersRes);
        }

        public async Task<IResult<IEnumerable<UserPublicResponseDto>>> SearchBySimilarityAsync(string text, CancellationToken token)
        {
            var usersRes = await _userRepository.SearchBySimilarityAsync(text, token);
            return MapToResponse(usersRes);
        }

        public async Task<IResult> RegisterAsync(UserRegisterRequestDto req)
        {
            var user = _mapper.Map<UserRegisterRequestDto, User>(req);
            user.RoleId = Constants.UserRoleId;
            return await _userRepository.CreateAsync(user);
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            return _userRepository.DeleteAsync(id);
        }

        IResult<IEnumerable<UserPublicResponseDto>> MapToResponse(IResult<IEnumerable<User>> result)
        {
            if (result.IsFailure) return result.FailureTo<IEnumerable<UserPublicResponseDto>>();
            var users = _mapper.Map<IEnumerable<User>, IEnumerable<UserPublicResponseDto>>(result.Result);
            return Result.Success(users);
        }
    }
}