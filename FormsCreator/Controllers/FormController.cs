using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Records;
using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.Form;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [Route("form")]
    public class FormController(IFormService service, ITemplateService templateService) : AbsController
    {
        private readonly IFormService _formService = service;
        private readonly ITemplateService _templateService = templateService;

        [HttpGet("~/forms/t/{templateId:guid}"), Authorize]
        public async Task<IActionResult> FormsByTemplateAsync(Guid templateId, CancellationToken token,
            int page = 1)
        {
            var formsRes = await _formService.GetByTemplateAsync(templateId, page, 10, token);
            if (formsRes.IsFailure) return CustomViewResponse(formsRes);
            var modelRes = await GetFormsPagingAsync(page, 10, formsRes.Result, null, templateId, token);
            if (modelRes.IsFailure) return CustomViewResponse(modelRes);
            return View(modelRes.Result);
        }

        [HttpGet("~/my-forms"), Authorize]
        public async Task<IActionResult> MyFormsAsync(CancellationToken token, int page = 1)
        {
            var formsRes = await _formService.GetByUserAsync(GetCurrentUserId(), page, 10, token);
            if (formsRes.IsFailure) return CustomViewResponse(formsRes);
            var modelRes = await GetFormsPagingAsync(page, 10, formsRes.Result, GetCurrentUserId(), null, token);
            if (modelRes.IsFailure) return CustomViewResponse(formsRes);
            return View(modelRes.Result);
        }

        [HttpGet("s/{id:guid}")]
        public async Task<IActionResult> SeeFormAsync(Guid id, CancellationToken token)
        {
            var res = await _formService.FindAsync(id, token);
            if (res.IsFailure) return CustomViewResponse(res);
            var confRes = await ConfigureViewAsync(res.Result.Template.Id, token);
            if (confRes.IsFailure) return CustomViewResponse(confRes);
            return View(res.Result);
        }

        [HttpGet("a/{templateId:guid}")]
        public async Task<IActionResult> AnswerTemplateAsync([FromRoute]Guid templateId, CancellationToken token)
        {
            var confRes = await ConfigureViewAsync(templateId, token);
            if (confRes.IsFailure) return CustomViewResponse(confRes);
            return View(new FormAddRequestDto());
        }

        [HttpPost("a/save"), Authorize]
        public async Task<IActionResult> CreateFormAsync([FromForm]FormAddRequestDto req,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                var confRes = await ConfigureViewAsync(req.TemplateId, token);
                if (confRes.IsFailure) return CustomViewResponse(confRes);
                return View("AnswerTemplate", req);
            }
            req.UserId = GetCurrentUserId();
            var res = await _formService.CreateAsync(req);
            if (res.IsFailure) return CustomViewResponse(res);
            return RedirectToAction("MyForms");
        }

        [HttpGet("e/{formId:guid}"), Authorize]
        public async Task<IActionResult> EditFormAsync([FromRoute]Guid formId, CancellationToken token)
        {
            var res = await _formService.FindAsUpdateAsync(formId, token);
            if (res.IsFailure) return CustomViewResponse(res);
            var confRes = await ConfigureViewAsync(res.Result.TemplateId, token);
            if (confRes.IsFailure) return CustomViewResponse(confRes);
            return View(res.Result);
        }

        [HttpPost("e/save"), Authorize]
        public async Task<IActionResult> UpdateFormAsync([FromForm]FormUpdateRequestDto req,
            CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                var confRes = await ConfigureViewAsync(req.TemplateId, token);
                if (confRes.IsFailure) return CustomViewResponse(confRes);
                return View("EditForm", req);
            }
            var res = await _formService.UpdateAsync(req);
            if (res.IsFailure) return CustomViewResponse(res);
            return RedirectToAction("MyForms");
        }

        /// <summary>
        /// This method is just for configure the views <see cref="EditFormAsync(Guid, CancellationToken)"/>,
        /// <see cref="AnswerTemplateAsync(Guid, CancellationToken)"/> and related to these methods.
        /// </summary>
        /// <param name="templateId">The template to search.</param>
        /// <param name="token">The token cancellation request.</param>
        /// <returns>An operation result.</returns>
        [NonAction]
        async Task<Core.Shared.IResult> ConfigureViewAsync(Guid templateId, CancellationToken token = default)
        {
            var tmplRes = await _templateService.FindAsync(templateId, token);
            if (tmplRes.IsFailure) return tmplRes;
            ViewData["Template"] = tmplRes.Result;
            return Result.Success();
        }

        [NonAction]
        private async Task<IResult<ShowPaging<FormResponseDto>>> GetFormsPagingAsync(int page, int size,
        IEnumerable<FormResponseDto> items, Guid? userId, Guid? templateId, CancellationToken token)
        {
            var totalRes = await GetCounterAsync(userId, templateId, token);
            if (totalRes.IsFailure)
                return totalRes.FailureTo<ShowPaging<FormResponseDto>>();

            return Result.Success(new ShowPaging<FormResponseDto>(items,
                new()
                {
                    CurrentPage = page,
                    TotalItems = totalRes.Result,
                    ItemsPerPage = size

                }));
        }

        [NonAction]
        Task<IResult<long>> GetCounterAsync(Guid? userId, Guid? templateId, CancellationToken token)
            => (userId.HasValue, templateId.HasValue) switch
            {
                (true, false) => _formService.CountByUserAsync(userId!.Value, token),
                (false, true) => _formService.CountByTemplateAsync(templateId!.Value, token),
                _ => Task.FromResult(Result.Success<long>(0))
            };
    }
}
