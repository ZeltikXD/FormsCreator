using AutoMapper;
using FormsCreator.Core.DTOs.Role;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class RoleService(IRoleRepository repository, IMapper mapper) : IRoleService
    {
        private readonly IRoleRepository _roleRepository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult<IEnumerable<RoleResponseDto>>> GetAllAsync(CancellationToken token)
        {
            var rolesRes = await _roleRepository.GetAllAsync(token);
            if (rolesRes.IsFailure) return rolesRes.FailureTo<IEnumerable<RoleResponseDto>>();
            var roles = _mapper.Map<IEnumerable<Role>, IEnumerable<RoleResponseDto>>(rolesRes.Result);
            return Result.Success(roles);
        }
    }
}
