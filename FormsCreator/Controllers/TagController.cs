using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [ApiController]
    [Route("api/v1/tags")]
    public class TagController(ITagService tagService) : AbsController
    {
        readonly ITagService _tagService = tagService;

        [HttpPost("add-new"), Authorize]
        public async Task<IActionResult> PostAsync([FromForm]TagDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _tagService.AddAsync(req);
            if (res.IsFailure) return CustomResponse(res);
            return Ok(new { NewId = res.Result });
        }
    }
}
