using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using FormsCreator.Application.Utils;
using FormsCreator.Core.Enums;

namespace FormsCreator.Controllers.Base
{
    /// <summary>
    /// Abstract controller with common functionality for this application.
    /// </summary>
    public abstract class AbsController : Controller
    {
        [NonAction]
        public RedirectResult ManageReturnUrl(string? returnUrl)
            => !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl) : Redirect(Url.Content("~/"));

        [NonAction]
        protected bool IsCurrentUserId(Guid userReqId)
            => GetCurrentUserId().Equals(userReqId);
        

        [NonAction]
        protected Guid GetCurrentUserId()
            => HttpContext.GetCurrentUserId();

        public IActionResult CustomResponse(Core.Shared.IResult result)
            => result.Error.Code switch
            {
                ResultErrorType.NullError => InternalError(result.Error),
                ResultErrorType.UnknownError => InternalError(result.Error),
                ResultErrorType.DatabaseError => InternalError(result.Error),
                ResultErrorType.AuthorizationError => Unauthorized(result.Error),
                ResultErrorType.ValidationError => BadRequest(result.Error),
                ResultErrorType.NotFoundError => NotFound(result.Error),
                ResultErrorType.ConflictError => Conflict(result.Error),
                ResultErrorType.UnprocessableEntityError => UnprocessableEntity(result.Error),
                ResultErrorType.None => NoContent(),
                _ => throw new NotImplementedException()
            };

        [NonAction]
        public IActionResult CustomViewResponse(Core.Shared.IResult result, object? model = null)
        {
            switch (result.Error.Code)
            {
                case ResultErrorType.NullError:
                    {
                        ModelState.AddModelError(string.Empty, result.Error.Message);
                        return View(model);
                    }
                case ResultErrorType.UnknownError:
                    {
                        return InternalError(result.Error);
                    }
                case ResultErrorType.DatabaseError:
                    {
                        return InternalError(result.Error);
                    }
                case ResultErrorType.AuthorizationError:
                    {
                        return Redirect("/");
                    }
                case ResultErrorType.ValidationError:
                    {
                        ModelState.AddModelError(string.Empty, result.Error.Message);
                        return View(model);
                    }
                case ResultErrorType.NotFoundError:
                    {
                        return NotFound();
                    }
                case ResultErrorType.ConflictError:
                    {
                        ModelState.AddModelError(string.Empty, result.Error.Message);
                        return View(model);
                    }
                case ResultErrorType.UnprocessableEntityError:
                    {
                        ModelState.AddModelError(string.Empty, result.Error.Message);
                        return View(model);
                    }
                case ResultErrorType.None:
                    {
                        return View(model);
                    }
                default:
                    return View(result);
            }
        }

        [NonAction]
        public ObjectResult InternalError([ActionResultObjectValue] object? value = null)
            => new(value) { StatusCode = StatusCodes.Status500InternalServerError };

        [NonAction]
        protected string GetLocalUrl(Uri? url)
        {
            if (url is null) return "/";

            return url.PathAndQuery;
        }
    }
}
