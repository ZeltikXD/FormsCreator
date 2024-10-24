using FormsCreator.Application.Abstractions;
using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.Comment;
using FormsCreator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [ApiController]
    [Route("api/v1/comments")]
    [Produces("application/json")]
    public class CommentController(ICommentService service) : AbsController
    {
        readonly ICommentService _service = service;

        [HttpGet("{templateId:guid}")]
        public async Task<IActionResult> GetCommentsAsync(Guid templateId, CancellationToken token,
            int page = 1, int size = 10)
        {
            var res = await _service.GetByTemplateAsync(templateId, page, size, token);
            if (res.IsFailure) return InternalError(res.Error);
            return Ok(res.Result);
        }

        [HttpGet("count/{templateId:guid}")]
        public async Task<IActionResult> GetCountAsync(Guid templateId, CancellationToken token)
        {
            var res = await _service.CountByTemplateAsync(templateId, token);
            if (res.IsFailure) return InternalError(res.Error);
            return Ok(new { Count = res.Result });
        }

        [HttpPost("add-to-template"), AutoValidateAntiforgeryToken, Authorize]
        public async Task<IActionResult> PostAsync([FromForm]CommentRequestDto req,
            [FromServices]ICommentNotifier notiService)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            req.UserId = GetCurrentUserId();
            var res = await _service.AddAsync(req);
            if (res.IsFailure) return InternalError(res.Error);
            await notiService.SendCommentPostedNotificationAsync(req.TemplateId).ConfigureAwait(false);
            return NoContent();
        }
    }
}
