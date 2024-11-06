using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Records;
using FormsCreator.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [Authorize]
    [Route("api/v1/atlassian/jira")]
    public class JiraController(IJiraService jiraService) : AbsController
    {
        private readonly IJiraService _jiraService = jiraService;

        [HttpPost("create-ticket"), Produces("application/json")]
        public Task<IActionResult> CreateTicketAsync([FromForm]JiraIssue issue)
        {
            return Task.FromResult<IActionResult>(new ObjectResult(new { Message = "Method not implemented." }) { StatusCode = StatusCodes.Status501NotImplemented });
            //if (!ModelState.IsValid) return BadRequest(ModelState);
            //issue.Referer = Request.GetTypedHeaders().Referer?.ToString() ?? string.Empty;
            //var result = await _jiraService.CreateTicketAsync(GetCurrentUserId(), issue);
            //if (result.IsFailure) return CustomResponse(result);
            //return Ok(result.Result);
        }
    }
}
