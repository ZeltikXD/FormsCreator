using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Records;
using FormsCreator.Application.Utils;
using FormsCreator.Controllers.Base;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    public class HomeController : AbsController
    {
        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromServices]ITemplateService tmplService,
            [FromServices]ITagService tagService, CancellationToken token)
        {
            var result = await GetIndexDataAsync(tmplService, tagService, token);
            if (result.IsFailure) return CustomViewResponse(result);
            return View(result.Result);
        }

        [HttpGet("change-lang/{lang}")]
        public IActionResult ChangeLanguage(SupportedLang lang)
        {
            if (!Enum.IsDefined(lang))
                return Redirect(GetLocalUrl(Request.GetTypedHeaders().Referer));

            Response.SetUserLanguage(lang);
            return Redirect(GetLocalUrl(Request.GetTypedHeaders().Referer));
        }

        [NonAction]
        static async Task<IResult<HomeIndexRecord>> GetIndexDataAsync(ITemplateService tmpService,
            ITagService tagService, CancellationToken token)
        {
            var popularRes = await tmpService.GetMostPopularAsync(1, 5, token);
            if (popularRes.IsFailure) return popularRes.FailureTo<HomeIndexRecord>();
            var recentRes = await tmpService.GetAllAsync(1, 5, token);
            if (recentRes.IsFailure) return recentRes.FailureTo<HomeIndexRecord>();
            var tagsRes = await tagService.GetAsync(15, token);
            if (tagsRes.IsFailure) return tagsRes.FailureTo<HomeIndexRecord>();
            return Result.Success<HomeIndexRecord>(new(popularRes.Result, recentRes.Result, tagsRes.Result));
        }
    }
}
