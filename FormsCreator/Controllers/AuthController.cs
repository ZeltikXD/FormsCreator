using FormsCreator.Controllers.Base;
using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace FormsCreator.Controllers
{
    [Route("auth")]
    public class AuthController(IAuthService authService) : AbsController
    {
        private readonly IAuthService _authService = authService;

        [HttpGet("login")]
        public IActionResult LogIn(string? returnUrl = null)
        {
            if (_authService.IsSignedIn())
                return ManageReturnUrl(returnUrl);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogInAsync([FromForm]UserLoginRequestDto req,
            CancellationToken token, string? returnUrl = null)
        {
            if (_authService.IsSignedIn())
                return ManageReturnUrl(returnUrl);

            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(req);
            }

            return await ManageSignInAsync(req, returnUrl, token);
        }

        [HttpGet("~/api/v1/auth/login/external/google")]
        public IActionResult LogInGoogle(string? returnUrl = null)
        {
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalCallback", new { state = returnUrl }),
                IssuedUtc = DateTimeOffset.UtcNow
            };

            return Challenge(authProperties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await _authService.SignOutAsync();

            return ManageReturnUrl("/");
        }

        [HttpGet("~/api/v1/auth/challenge/external/callback")]
        public async Task<IActionResult> ExternalCallbackAsync(string? state = null)
        {
            var res = await _authService.SignInExternalAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (res.IsFailure) return CustomResponse(res);
            return ManageReturnUrl(state);
        }

        [HttpGet("register")]
        public IActionResult RegisterAsync([FromQuery] string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync([FromForm] UserRegisterRequestDto req,
            [FromServices] IUserService userService,
            [FromServices] IEmailSender emailSender,
            [FromQuery] string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View(req);
            }

            var result = await userService.RegisterAsync(req);
            if (result.IsFailure) return CustomViewResponse(result);

            //var bodyRes = await PrepareConfirmationBodyAsync(result.Result, requestDto.UserName, returnUrl, userService);

            //if (bodyRes.IsFailure) return MakeResponse(bodyRes.Errors);

            //await emailSender.SendEmailAsync(requestDto.Email, "Confirme su registro", bodyRes.Result, true);

            return RedirectToAction(nameof(PostRegistration));
        }

        [HttpGet("confirm-registration")]
        public IActionResult PostRegistration()
        {
            return RedirectToAction(nameof(LogIn));
        }

        [HttpGet("~/signin-google")]
        public IActionResult DummyLink()
        {
            return Redirect("/");
        }

        [NonAction]
        private async Task<IActionResult> ManageSignInAsync(UserLoginRequestDto request, string? returnUrl, CancellationToken token)
        {
            var res = await _authService.SignInAsync(request, token);

            if (res.IsSuccess) return ManageReturnUrl(returnUrl);

            ModelState.AddModelError(string.Empty, res.Error.Message);
            return View(request);
        }
    }
}
