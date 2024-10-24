using FormsCreator.Application.Abstractions;
using FormsCreator.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [ApiController]
    [Route("api/v1/events")]
    public class SseController(ICommentNotifier service) : AbsController
    {
        [HttpGet("notificate-comment/{templateId:guid}")]
        [Produces("text/event-stream")]
        public async Task<IActionResult> StartEvent(Guid templateId, CancellationToken token)
        {
            var userId = Guid.NewGuid();
            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");

            var clientDisconnected = new TaskCompletionSource();
            token.Register(clientDisconnected.SetResult);

            service.RegisterClient(templateId, userId, Response);

            await clientDisconnected.Task;

            service.UnregisterClient(templateId, userId);

            return Ok();
        }
    }
}
