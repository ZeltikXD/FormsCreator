using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.TemplateAccess;
using FormsCreator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [ApiController, Authorize]
    [Route("api/v1/template-accesses")]
    public class TemplateAccessController(ITemplateAccessService service) : AbsController
    {
        readonly ITemplateAccessService _service = service;

        [HttpPost("add-permission")]
        public async Task<IActionResult> PostAsync([FromForm]TemplateAccessRequestDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _service.AddAsync(req);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }

        [HttpPost("add-permissions")]
        public async Task<IActionResult> PostRangeAsync([FromForm]TemplateAccessRequestRangeDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _service.AddRangeAsync(req.UserIds, req.TemplateId);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromForm]TemplateAccessRequestDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _service.DeleteAsync(req.UserId, req.TemplateId);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }

        [HttpDelete("delete-range")]
        public async Task<IActionResult> DeleteRangeAsync([FromForm]TemplateAccessRequestRangeDto req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var res = await _service.DeleteRangeAsync(req.UserIds, req.TemplateId);
            if (res.IsFailure) return CustomResponse(res);
            return Ok();
        }
    }
}