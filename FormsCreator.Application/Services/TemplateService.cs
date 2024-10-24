using AutoMapper;
using FormsCreator.Application.Abstractions;
using FormsCreator.Core.DTOs.Question;
using FormsCreator.Core.DTOs.QuestionOption;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.DTOs.Template;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Http;

namespace FormsCreator.Application.Services
{
    internal class TemplateService(ITemplateRepository repository,
        IImageManager imageManager, ITagRepository tagRepository,
        IQuestionRepository questionRepository, ILikeRepository likeRepository,
        IFormRepository formRepository, IQuestionOptionRepository questionOptRepository,
        ICommentRepository cmmRepo, IAuthService authService,
        IMapper mapper) : ITemplateService
    {
        private readonly ITemplateRepository _repository = repository;
        private readonly IQuestionRepository _questionRepository = questionRepository;
        private readonly IImageManager _imageManager = imageManager;
        private readonly ITagRepository _tagRepository = tagRepository;
        private readonly IFormRepository _formRepository = formRepository;
        private readonly ILikeRepository _likeRepository = likeRepository;
        private readonly IQuestionOptionRepository _questionOptRepository = questionOptRepository;
        private readonly ICommentRepository _commentRepository = cmmRepo;
        private readonly IAuthService _authService = authService;
        private readonly IMapper _mapper = mapper;

        public Task<IResult<long>> CountAllAsync(CancellationToken token)
        {
            return _repository.CountAllAsync(token);
        }

        public Task<IResult<long>> CountByCreatorAsync(Guid userId, CancellationToken token)
        {
            return _repository.CountByCreatorAsync(userId, token);
        }

        public Task<IResult<long>> CountByTopicsAsync(string[] topics, CancellationToken token)
        {
            return _repository.CountByTopicsAsync(topics, token);
        }

        public Task<IResult<long>> CountByTagsAsync(string[] tags, CancellationToken token)
        {
            return _repository.CountByTagsAsync(tags, token);
        }

        public Task<IResult<long>> CountByTextAsync(string text, SupportedLang lang, CancellationToken token)
        {
            return _repository.CountByTextAsync(text, lang, token);
        }

        public async Task<Core.Shared.IResult> CreateAsync(TemplateCreateRequestDto<IFormFile> template)
        {
            var templ = _mapper.Map<TemplateCreateRequestDto<IFormFile>, Template>(template);
            var manageTagsTask = ManageTagsAsync(template.Tags);
            templ.Image_Url = await GetImageUrlAsync(template.Image);
            templ.Tags = await manageTagsTask;
            return await _repository.CreateAsync(templ);
        }

        public Task<Core.Shared.IResult> DeleteAsync(Guid id)
        {
            return _repository.DeleteAsync(id);
        }

        public async Task<IResult<TemplateResponseDto>> FindAsync(Guid id, CancellationToken token)
        {
            var templRes = await _repository.FindAsync(id, token);
            if (templRes.IsFailure) return templRes.FailureTo<TemplateResponseDto>();
            var templ = _mapper.Map<Template, TemplateResponseDto>(templRes.Result);
            templ.Questions = await GetQuestionsAsync(id, token);
            await GetTemplatesLikesAndFormsAndCommentsAsync([templ], token);
            return Result.Success(templ);
        }

        public async Task<IResult<TemplateUpdateRequestDto<IFormFile>>> FindAsUpdateAsync(Guid id, CancellationToken token)
        {
            var res = await FindAsync(id, token);
            if (res.IsFailure) return res.FailureTo<TemplateUpdateRequestDto<IFormFile>>();
            var tmpl = _mapper.Map<TemplateResponseDto, TemplateUpdateRequestDto<IFormFile>>(res.Result);
            return ManagePermission(res.Result.Creator.Id, tmpl);
        }

        public async Task<IResult<IEnumerable<TemplateResponseDto>>> GetAllAsync(int page, int size, CancellationToken token)
        {
            var templsRes = await _repository.GetAllAsync(page, size, token);
            return await MapToResponseAsync(templsRes, token);
        }

        public async Task<IResult<IEnumerable<TemplateResponseDto>>> GetByCreatorAsync(Guid userId, int page, int size, CancellationToken token)
        {
            var templsRes = await _repository.GetByCreatorAsync(userId, page, size, token);
            return await MapToResponseAsync(templsRes, token);
        }

        public async Task<IResult<IEnumerable<TemplateResponseDto>>> GetByTextAsync(int page, int size, string text, SupportedLang lang, CancellationToken token)
        {
            var templsRes = await _repository.GetByTextAsync(page, size, text, lang, token);
            return await MapToResponseAsync(templsRes, token);
        }

        public async Task<IResult<IEnumerable<TemplateResponseDto>>> GetByTopicsAsync(int page, int size, string[] topics, CancellationToken token)
        {
            var templsRes = await _repository.GetByTopicsAsync(page, size, topics, token);
            return await MapToResponseAsync(templsRes, token);
        }

        public async Task<IResult<IEnumerable<TemplateResponseDto>>> GetByTagsAsync(int page, int size, string[] tags, CancellationToken token)
        {
            var templsRes = await _repository.GetByTagsAsync(page, size, tags, token);
            return await MapToResponseAsync(templsRes, token);
        }

        public async Task<IResult<IEnumerable<TemplateResponseDto>>> GetMostPopularAsync(int page, int size, CancellationToken token)
        {
            var templsRes = await _repository.GetMostPopularAsync(page, size, token);
            return await MapToResponseAsync(templsRes, token);
        }

        public async Task<Core.Shared.IResult> UpdateAsync(TemplateUpdateRequestDto<IFormFile> template)
        {
            var perm = await ComprobatePermissionAsync(template.Id);
            if (perm.IsFailure) return perm;
            var templ = _mapper.Map<TemplateUpdateRequestDto<IFormFile>, Template>(template);
            var manageTagsTask = ManageTagsAsync(template.Tags);
            await ManageImageUpdateAsync(template, templ);
            templ.Tags = await manageTagsTask;
            var res = await _repository.UpdateAsync(templ);
            if (res.IsFailure) return res;
            return await ManageQuestionsUpdateAsync(template.Id, template.Questions);
        }

        async Task<Core.Shared.IResult> ComprobatePermissionAsync(Guid templateId)
        {
             var templRes = await _repository.FindAsync(templateId);
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

        async Task<string> GetImageUrlAsync(IFormFile? imageSrc)
        {
            if (imageSrc is null) return _imageManager.DefaultImage;
            await using var image = imageSrc.OpenReadStream();
            var result = await _imageManager.UploadAsync(imageSrc.Name, image);
            return result.IsFailure ? _imageManager.DefaultImage
                : result.Result;
        }

        async Task ManageImageUpdateAsync(TemplateUpdateRequestDto<IFormFile> req, Template current)
        {
            if (req.Image is null && req.DeleteCurrentImage)
            {
                current.Image_Url = _imageManager.DefaultImage;
                return;
            }
            var templ = await _repository.FindAsync(req.Id);
            if (templ.IsFailure) return;
            if (!req.DeleteCurrentImage) { current.Image_Url = templ.Result.Image_Url; return; }
            await _imageManager.DeleteAsync(templ.Result.Image_Url);
            current.Image_Url = await GetImageUrlAsync(req.Image);
        }

        async Task<IList<QuestionResponseDto>> GetQuestionsAsync(Guid templateId, CancellationToken token)
        {
            var res = await _questionRepository.GetByTemplateAsync(templateId, token);
            if (res.IsFailure) return [];
            var questions = _mapper.Map<IEnumerable<Question>, IList<QuestionResponseDto>>(res.Result);
            await GetQuestionsOptions(questions, token);
            return questions;
        }

        async Task GetQuestionsOptions(IEnumerable<QuestionResponseDto> questions, CancellationToken token)
        {
            foreach (var question in questions)
            {
                var res = await _questionOptRepository.GetByQuestionAsync(question.Id, token);
                if (res.IsFailure) continue;
                question.Options = _mapper.Map<IEnumerable<QuestionOption>, IList<QuestionOptionResponseDto>>(res.Result);
            }
        }

        async Task<Core.Shared.IResult> ManageQuestionsUpdateAsync(Guid templateId, IEnumerable<QuestionRequestDto> questions)
        {
            var res = await _repository.HasFormsFilledOutAsync(templateId);
            if (res.IsFailure) return Result.Success();
            var qs = _mapper.Map<IEnumerable<QuestionRequestDto>, IEnumerable<Question>>(questions);
            AddTemplateIdToQuestions(templateId, qs);
            var updRes = await _questionRepository.UpdateRangeAsync(qs);
            await ManageQuestionsOptionsUpdateAsync(qs);
            return updRes;
        }

        async Task ManageQuestionsOptionsUpdateAsync(IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                await ManageQuestionOptionsUpdateAsync(question.Id, question.Options);
            }
        }

        async Task<Core.Shared.IResult> ManageQuestionOptionsUpdateAsync(Guid questionId, IEnumerable<QuestionOption> options)
        {
            AddQuestionIdToOptions(questionId, options);
            return await _questionOptRepository.UpdateRangeAsync(options);
        }

        static void AddQuestionIdToOptions(Guid id, IEnumerable<QuestionOption> options)
        {
            foreach (var option in options)
            {
                option.QuestionId = id;
            }
        }

        static void AddTemplateIdToQuestions(Guid id, IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                question.TemplateId = id;
            }
        }

        async Task<ICollection<Tag>> ManageTagsAsync(ICollection<TagDto> tags)
        {
            List<Tag> trackedTags = [];
            foreach (var tag in tags)
            {
                var res = await (tag.Id == default ? _tagRepository.FindAsync(tag.Name) 
                    : _tagRepository.FindAsync(tag.Id));
                if (res.IsFailure) continue;
                trackedTags.Add(res.Result);
            }
            return trackedTags;
        }

        async Task<IResult<IEnumerable<TemplateResponseDto>>> MapToResponseAsync(IResult<IEnumerable<Template>> templsRes, CancellationToken token)
        {
            if (templsRes.IsFailure) return templsRes.FailureTo<IEnumerable<TemplateResponseDto>>();
            var templates = _mapper.Map<IEnumerable<Template>, IEnumerable<TemplateResponseDto>>(templsRes.Result);
            await GetTemplatesLikesAndFormsAndCommentsAsync(templates, token);
            return Result.Success(templates);
        }

        async Task GetTemplatesLikesAndFormsAndCommentsAsync(IEnumerable<TemplateResponseDto> templates, CancellationToken token)
        {
            foreach (var template in templates)
            {
                var res = await _formRepository.CountByTemplateAsync(template.Id, token);
                if (res.IsFailure) continue;
                var res2 = await _likeRepository.CountByTemplateAsync(template.Id, token);
                if (res2.IsFailure) continue;
                var res3 = await _commentRepository.CountByTemplateAsync(template.Id, token);
                if (res3.IsFailure) continue;
                template.LikesCount = res2.Result;
                template.FormsCount = res.Result;
                template.CommentsCount = res3.Result;
            }
        }
    }
}
