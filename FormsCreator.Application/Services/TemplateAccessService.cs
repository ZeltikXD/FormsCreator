using AutoMapper;
using FormsCreator.Core.DTOs.TemplateAccess;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class TemplateAccessService(ITemplateAccessRepository repository,
        IAuthService authService, ITemplateRepository templateRepository, 
        IMapper mapper) : ITemplateAccessService
    {
        private readonly ITemplateAccessRepository _repository = repository;
        private readonly ITemplateRepository _templateRepository = templateRepository;
        private readonly IAuthService _authService = authService;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult> AddAsync(TemplateAccessRequestDto req)
        {
            var comprobation = await ComprobatePermissionAsync(req.TemplateId);
            if (!comprobation.IsFailure) return comprobation;
            var permRes = await _repository.UserHasPermissionAsync(req.UserId, req.TemplateId);
            if (permRes.IsFailure) return permRes;
            if (permRes.Result) return permRes;
            var access = _mapper.Map<TemplateAccessRequestDto, TemplateAccess>(req);
            return await _repository.CreateAsync(access);
        }

        public async Task<IResult> AddRangeAsync(IEnumerable<Guid> userids, Guid templateId)
        {
            var comprobation = await ComprobatePermissionAsync(templateId);
            if (!comprobation.IsFailure) return comprobation;
            return await _repository.CreateRangeAsync(userids.Select(x => new TemplateAccess { UserId = x, TemplateId = templateId }));
        }

        public async Task<IResult> DeleteAsync(Guid userId, Guid templateId)
        {
            var comprobation = await ComprobatePermissionAsync(templateId);
            if (!comprobation.IsFailure) return comprobation;
            var permRes = await _repository.UserHasPermissionAsync(userId, templateId);
            if (permRes.IsFailure) return permRes;
            if (!permRes.Result) return permRes;
            return await _repository.DeleteAsync(userId, templateId);
        }

        public async Task<IResult> DeleteRangeAsync(IEnumerable<Guid> userids, Guid templateId)
        {
            var comprobation = await ComprobatePermissionAsync(templateId);
            if (!comprobation.IsFailure) return comprobation;
            return await _repository.DeleteRangeAsync(userids.Select(x => new TemplateAccess { UserId = x, TemplateId = templateId }));
        }

        async Task<IResult> ComprobatePermissionAsync(Guid templateId)
        {
            var templRes = await _templateRepository.FindAsync(templateId);
            if (templRes.IsFailure) return templRes;
            return ManagePermission(templRes.Result.CreatorId, 0);
        }


        /// <typeparam name="T"></typeparam>
        /// <param name="userId">The user id inside the object.</param>
        /// <param name="returnObject">The object to return</param>
        /// <returns>The operation result.</returns>
        IResult<T> ManagePermission<T>(Guid userId, T returnObject)
        {
            if (_authService.GetUserId() != userId && !_authService.HasRole("Admin"))
            {
                return Result.Failure<T>(new(ResultErrorType.AuthorizationError, string.Empty));
            }
            return Result.Success(returnObject);
        }
    }
}
