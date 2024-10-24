using AutoMapper;
using FormsCreator.Core.DTOs.Answer;
using FormsCreator.Core.DTOs.AnswerOption;
using FormsCreator.Core.DTOs.Form;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class FormService(IFormRepository formRepository,
        IAnswerRepository answerRepository, IAnswerOptionRepository answerOptRepo,
        ITemplateAccessRepository templateAccess, IAuthService authService,
        IMapper mapper) : IFormService
    {
        private readonly IFormRepository _formRepository = formRepository;
        private readonly IAnswerRepository _answerRepository = answerRepository;
        private readonly IAnswerOptionRepository _answerOptRepository = answerOptRepo;
        private readonly ITemplateAccessRepository _templateAccessRepository = templateAccess;
        private readonly IAuthService _authService =  authService;
        private readonly IMapper _mapper = mapper;

        public Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token)
        {
            return _formRepository.CountByTemplateAsync(templateId, token);
        }

        public Task<IResult<long>> CountByUserAsync(Guid userId, CancellationToken token)
        {
            return _formRepository.CountByUserAsync(userId, token);
        }

        public async Task<IResult> CreateAsync(FormAddRequestDto formReq)
        {
            var form = _mapper.Map<FormAddRequestDto, Form>(formReq);
            var perm = await ManagePermissionAsync(formReq.UserId, formReq.TemplateId, 0);
            if (perm.IsFailure) return perm;
            return await _formRepository.CreateAsync(form);
        }

        public async Task<IResult<FormResponseDto>> FindAsync(Guid id, CancellationToken token)
        {
            var formRes = await GetFormWithAnswersAsync(id, token);
            if (formRes.IsFailure) return formRes.FailureTo<FormResponseDto>();
            var form = _mapper.Map<Form, FormResponseDto>(formRes.Result);
            return await ManagePermissionAsync(_authService.GetUserId(), formRes.Result.TemplateId, form, token);
        }

        public async Task<IResult<FormUpdateRequestDto>> FindAsUpdateAsync(Guid id, CancellationToken token)
        {
            var formRes = await GetFormWithAnswersAsync(id, token);
            if (formRes.IsFailure) return formRes.FailureTo<FormUpdateRequestDto>();
            var form = _mapper.Map<Form, FormUpdateRequestDto>(formRes.Result);
            return Result.Success(form);
        }

        public async Task<IResult<IEnumerable<FormResponseDto>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token)
        {
            var formsRes = await _formRepository.GetByTemplateAsync(templateId, page, size, token);
            if (formsRes.IsFailure) return formsRes.FailureTo<IEnumerable<FormResponseDto>>();
            var forms = _mapper.Map<IEnumerable<Form>, IEnumerable<FormResponseDto>>(formsRes.Result);
            return Result.Success(forms);
        }

        public async Task<IResult<IEnumerable<FormResponseDto>>> GetByUserAsync(Guid userId, int page, int size, CancellationToken token)
        {
            var formsRes = await _formRepository.GetByUserAsync(userId, page, size, token);
            if (formsRes.IsFailure) return formsRes.FailureTo<IEnumerable<FormResponseDto>>();
            var forms = _mapper.Map<IEnumerable<Form>, IEnumerable<FormResponseDto>>(formsRes.Result);
            return Result.Success(forms);
        }

        public async Task<IResult> UpdateAsync(FormUpdateRequestDto form)
        {
            var formExists = await _formRepository.ExistsAsync(form.Id);
            if (formExists.IsFailure) return formExists;
            var result = await ManageAnswersUpdateAsync(form.GetAnswersWithFormId());            
            if (result.IsFailure) return result;
            return Result.Success();
        }

        async Task<IResult<T>> ManagePermissionAsync<T>(Guid userId, Guid templateId, T objectRes, CancellationToken token = default)
        {
            var res = await _templateAccessRepository.UserHasPermissionAsync(userId, templateId, token);
            if (res.IsFailure) return res.FailureTo<T>();
            if (!res.Result && !_authService.HasRole("Admin"))
            {
                return Result.Failure<T>(new(ResultErrorType.AuthorizationError, string.Empty));
            }
            return Result.Success(objectRes);
        }

        async Task<IResult<Form>> GetFormWithAnswersAsync(Guid id, CancellationToken token)
        {
            var formRes = await _formRepository.FindAsync(id, token);
            if (formRes.IsFailure) return formRes;
            var answersRes = await _answerRepository.GetByFormAsync(id, token);
            if (answersRes.IsFailure) return answersRes.FailureTo<Form>();
            formRes.Result.Answers = answersRes.Result.ToList();
            return formRes;
        }

        async Task<IResult> ManageAnswersUpdateAsync(IEnumerable<AnswerRequestDto> answers)
        {
            var areFromForm = await _answerRepository.AreFromFormAsync(answers.First().FormId, answers.Select(x => x.Id));
            if (!areFromForm) return Result.Failure(new(ResultErrorType.ValidationError, "The answers must be from the current form."));
            return await UpdateAnswerOptionsAsync(answers);
        }

        async Task<IResult> UpdateAnswerOptionsAsync(IEnumerable<AnswerRequestDto> answers)
        {
            foreach (var answersItem in answers)
            {
                var options = _mapper.Map<IEnumerable<AnswerOptionDto>, IEnumerable<AnswerOption>>(answersItem.GetOptionsWithAnswerId());
                var res = await _answerOptRepository.UpdateRangeAsync(options);
                if (res.IsFailure) continue;
            }
            return Result.Success();
        }
    }
}
