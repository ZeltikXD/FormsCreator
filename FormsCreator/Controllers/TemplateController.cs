using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Records;
using FormsCreator.Application.Utils;
using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.Template;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsCreator.Controllers
{
    [Route("templates")]
    public class TemplateController(ITemplateService templateService) : AbsController
    {
        private readonly ITemplateService _templateService = templateService;

        [HttpGet("~/my-templates"), Authorize]
        public async Task<IActionResult> MyTemplatesAsync(CancellationToken token, int page = 1, int size = 6)
        {
            var res = await _templateService.GetByCreatorAsync(GetCurrentUserId(), page, size, token);
            if (res.IsFailure) return CustomViewResponse(res);
            var modelRes = await GetTemplatesPagingAsync(page, size, res.Result, GetCurrentUserId(), [], [], null, token);
            if (modelRes.IsFailure) return CustomViewResponse(modelRes);
            return View(modelRes.Result);
        }

        [HttpGet]
        public async Task<IActionResult> TemplatesAsync([FromServices]ITopicService topicService,
            [FromServices]ITagService tagService, CancellationToken token, 
            string? tags = null, string? topics = null, string? text = null, int page = 1)
        {
            var res = await GetTemplatesAsync(page, 10, null, topics.ToParameterArray(), tags.ToParameterArray(), text, token);
            if (res.IsFailure) return CustomViewResponse(res);
            var modelRes = await GetTemplatesPagingAsync(page, 10, res.Result, null, topics.ToParameterArray(), tags.ToParameterArray(), text, token);
            if (modelRes.IsFailure) return CustomViewResponse(modelRes);
            await ConfigureTemplatesViewAsync(topicService, tagService, topics.ToParameterArray(), tags.ToParameterArray(), text, token);
            return View(modelRes.Result);
        }

        [HttpGet("create"), Authorize]
        public async Task<IActionResult> CreateAsync([FromServices]ITopicService topicService,
            [FromServices]ITagService tagService, CancellationToken token)
        {
            await CommonViewConfigureAsync(topicService, tagService, default, [], token);
            return View(new TemplateCreateRequestDto<IFormFile>());
        }

        [HttpPost("create"), Authorize, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([FromForm] TemplateCreateRequestDto<IFormFile> req,
            [FromServices] ITagService tagService, [FromServices] ITopicService topicService,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                await CommonViewConfigureAsync(topicService, tagService, req.TopicId, req.Tags.Select(x => x.Id).ToArray(), token);
                return View(req);
            }
            req.CreatorId = GetCurrentUserId();
            var res = await _templateService.CreateAsync(req);
            if (res.IsFailure) return CustomViewResponse(res);
            return RedirectToAction("MyTemplates");
        }

        [HttpGet("edit/{templateId:guid}"), Authorize]
        public async Task<IActionResult> EditAsync(Guid templateId, [FromServices] ITopicService topicService,
            [FromServices] ITagService tagService, CancellationToken token)
        {
            var res = await _templateService.FindAsUpdateAsync(templateId, token);
            if (res.IsFailure) return CustomViewResponse(res);
            await CommonViewConfigureAsync(topicService, tagService, default, res.Result.Tags.Select(x => x.Id).ToArray(), token);
            return View(res.Result);
        }

        [HttpPost("edit"), Authorize]
        public async Task<IActionResult> UpdateAsync([FromForm]TemplateUpdateRequestDto<IFormFile> req,
            [FromServices] ITopicService topicService, [FromServices] ITagService tagService,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                await CommonViewConfigureAsync(topicService, tagService, req.TopicId, req.Tags.Select(x => x.Id).ToArray(), token);
                var templRes = await _templateService.FindAsUpdateAsync(req.Id, token);
                if (templRes.IsFailure) return CustomViewResponse(templRes);
                return View("Edit", templRes.Result);
            }
            var res = await _templateService.UpdateAsync(req);
            if (res.IsFailure) return CustomViewResponse(res);
            return RedirectToAction("MyTemplates");
        }

        [NonAction]
        private async Task<Core.Shared.IResult> ConfigureTemplatesViewAsync(ITopicService topicService,
            ITagService tagService, string[] topics, string[] tags, string? text, CancellationToken token)
        {
            var topicsRes = await topicService.GetAllAsync(token);
            if (topicsRes.IsFailure) return topicsRes;
            ViewData["Topics"] = new MultiSelectList(topicsRes.Result, "Name", "Name", topics);
            var tagsRes = await tagService.GetAsync(token);
            if (tagsRes.IsFailure) return tagsRes;
            ViewData["Tags"] = new MultiSelectList(tagsRes.Result, "Name", "Name", tags);
            ViewData["SearchText"] = text;
            return Result.Success();
        }

        [NonAction]
        private async Task<Core.Shared.IResult> CommonViewConfigureAsync(ITopicService topicService,
            ITagService tagService, Guid topicId, Guid[] tagsId, CancellationToken token)
        {
            var topicsRes = await topicService.GetAllAsync(token);
            if (topicsRes.IsFailure) return topicsRes;
            ViewData["Topics"] = new SelectList(topicsRes.Result, "Id", "Name", topicId);
            var tagsRes = await tagService.GetAsync(token);
            if (tagsRes.IsFailure) return tagsRes;
            ViewData["Tags"] = new MultiSelectList(tagsRes.Result, "Id", "Name", tagsId);
            return Result.Success();
        }

        [NonAction]
        private async Task<IResult<ShowPaging<TemplateResponseDto>>> GetTemplatesPagingAsync(int page, int size,
            IEnumerable<TemplateResponseDto> items, Guid? userId, string[] topics, string[] tags, string? text, CancellationToken token)
        {
            var totalRes = await GetCounterAsync(userId, topics, tags, text, token);
            if (totalRes.IsFailure)
                return totalRes.FailureTo<ShowPaging<TemplateResponseDto>>();

            return Result.Success(new ShowPaging<TemplateResponseDto>(items,
                new()
                {
                    CurrentPage = page,
                    TotalItems = totalRes.Result,
                    ItemsPerPage = size
         
                }));
        }

        [NonAction]
        private Task<IResult<long>> GetCounterAsync(Guid? userId, string[] topics,
            string[] tags, string? text, CancellationToken token)
            => (userId.HasValue, topics.Length > 0, tags.Length > 0, text is not null) switch
            {
                (true, false, false, false) => _templateService.CountByCreatorAsync(userId!.Value, token),
                (false, true, false, false) => _templateService.CountByTopicsAsync(topics!, token),
                (false, false, true, false) => _templateService.CountByTagsAsync(tags!, token),
                (false, false, false, true) => _templateService.CountByTextAsync(text!, Request.GetUserLanguage(), token),
                _ => _templateService.CountAllAsync(token)
            };

        [NonAction]
        private Task<IResult<IEnumerable<TemplateResponseDto>>> GetTemplatesAsync(int page, int size, Guid? userId, string[] topics,
            string[] tags, string? text, CancellationToken token)
        => (userId.HasValue, topics.Length > 0, tags.Length > 0, text is not null) switch
        {
            (true, false, false, false) => _templateService.GetByCreatorAsync(userId!.Value, page, size, token),
            (false, true, false, false) => _templateService.GetByTopicsAsync(page, size, topics, token),
            (false, false, true, false) => _templateService.GetByTagsAsync(page, size, tags, token),
            (false, false, false, true) => _templateService.GetByTextAsync(page, size, text!, Request.GetUserLanguage(), token),
            _ => _templateService.GetAllAsync(page, size, token)
        };
    }
}
