using FormsCreator.Application.Options;
using FormsCreator.Application.Resources;
using FormsCreator.Application.Utils;
using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IResult = FormsCreator.Core.Shared.IResult;

namespace FormsCreator.Application.Services
{
    internal class AuthService(IUserRepository userRepository,
        IOptions<TokenOptions> options, IHttpContextAccessor contextAccessor) : IAuthService
    {
        private readonly HttpContext _context = contextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(contextAccessor), "This class should be instantiated when an HTTP request occurs.");
        private readonly IUserRepository _userRepository = userRepository;
        private readonly TokenOptions _tokenOptions = options.Value;

        public async Task<IResult> SignInAsync(UserLoginRequestDto request, CancellationToken token)
        {
            var comprobateProviders = await ComprobateProvidersAsync(request.UserOrEmail);
            if (comprobateProviders.IsFailure) return comprobateProviders;
            var result = await _userRepository.IsPassCorrectAsync(request.UserOrEmail, request.Password, request.IsEmail, token);
            if (result.IsFailure) return result;
            var res = await ManageTokenCreationAsync(request, token);
            if (res.IsFailure) return res;

            return Result.Success();
        }

        public async Task SignOutAsync()
        {
            _context.Response.Cookies.Delete("session_token");
            var authenticationScheme = _context.User.FindFirstValue(ClaimTypes.AuthenticationMethod);
            if (!string.IsNullOrEmpty(authenticationScheme) && !authenticationScheme.Contains("Bearer"))
                await _context.SignOutAsync(authenticationScheme);
        }

        public bool HasRole(string roleName)
            => _context.User.IsInRole(roleName);

        public bool IsSignedIn()
        {
            if (_context.Request.Cookies.TryGetValue("session_token", out _))
                return true;

            return _context.User.Identity?.IsAuthenticated ?? false;
        }

        public string GetUserName()
            => _context.GetCurrrentUserName();

        public Guid GetUserId()
            => _context.GetCurrentUserId();

        public async Task<IResult> SignInExternalAsync(string scheme)
        {
            var res = await _context.AuthenticateAsync(scheme);
            if (!res.Succeeded) return Result.Failure(new(ResultErrorType.AuthorizationError, res.Failure?.Message ?? string.Empty));
            return await ManageRegisterAsync(res.Principal, scheme);
        }

        private static CookieOptions GetDefaultOptions(DateTimeOffset expiresIn)
            => new()
            {
                SameSite = SameSiteMode.Strict,
                Secure = true,
                HttpOnly = true,
                Expires = expiresIn,
                IsEssential = true
            };

        string CreateToken(User user, out DateTimeOffset expTime)
        {
            var key = Encoding.Unicode.GetBytes(_tokenOptions.SecretKey);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.GivenName, user.UserName));
            claims.AddClaim(new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(_tokenOptions.ExpiresInHours),
                Audience = _tokenOptions.Audience,
                Issuer = _tokenOptions.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            expTime = tokenDescriptor.Expires.Value;
            return tokenHandler.WriteToken(createdToken);
        }

        async Task<IResult> ComprobateProvidersAsync(string nameOrEmail)
        {
            var res = await _userRepository.HasProvidersAsync(nameOrEmail);
            if (res.IsFailure) return res;
            return !res.Result ? Result.Success()
                : Result.Failure(new(ResultErrorType.ValidationError, ValidationMessages.UserExternalService));
        }

        async Task<IResult> ManageTokenCreationAsync(UserLoginRequestDto request, CancellationToken token)
        {
            var userRes = await (request.IsEmail ? _userRepository.FindByEmailAsync(request.UserOrEmail, token)
                : _userRepository.FindByNameAsync(request.UserOrEmail, token));
            if (userRes.IsFailure) return userRes;
            var usrToken = CreateToken(userRes.Result, out var expTime);
            _context.Response.Cookies.Append("session_token", usrToken, GetDefaultOptions(expTime));
            return Result.Success();
        }

        async Task<IResult> ManageRegisterAsync(ClaimsPrincipal? claims, string scheme)
        {
            if (claims is null) return Result.Failure(new(ResultErrorType.AuthorizationError, string.Empty));
            var name = claims.FindFirstValue(ClaimTypes.GivenName)!;
            var email = claims.FindFirstValue(ClaimTypes.Email)!;
            var provider = claims.Identity?.AuthenticationType ?? scheme;
            var manageRes = await ManageUserAsync(email, claims, scheme);
            if (manageRes.IsSuccess) return manageRes;
            var res = await _userRepository.CreateAsync(CreateUser(name, email, provider));
            if (res.IsFailure) return res;
            return await ManageUserAsync(email, claims, scheme);
        }

        async Task SignInAsync(ClaimsPrincipal oldPrincipal, string scheme, User user)
        {
            var newClaims = GetClaims(oldPrincipal, user, scheme);
            var identity = new ClaimsIdentity(newClaims, scheme);
            await _context.SignOutAsync(scheme);
            await _context.SignInAsync(scheme, new ClaimsPrincipal(identity));
        }

        static User CreateUser(string name, string email, string provider)
        {
            var user = new User
            {
                UserName = name,
                Email = email,
                IsEmailConfirmed = true,
                RoleId = Constants.UserRoleId,
                Providers = [new() { Provider = provider }]
            };
            return user;
        }

        async Task<IResult> ManageUserAsync(string email, ClaimsPrincipal principal, string scheme)
        {
            var userRes = await _userRepository.FindByEmailAsync(email);
            if (userRes.IsFailure) return userRes;
            await SignInAsync(principal, scheme, userRes.Result);
            return Result.Success();
        }

        static List<Claim> GetClaims(ClaimsPrincipal claims, User user, string scheme)
        {
            List<Claim> currentClaims = new(claims.Claims);
            var nameIdentifier = currentClaims.Find(x => x.Type == ClaimTypes.NameIdentifier);
            var givenName = currentClaims.Find(x => x.Type == ClaimTypes.GivenName);
            if (nameIdentifier != null)
                currentClaims.Remove(nameIdentifier);
            if (givenName is not null)
                currentClaims.Remove(givenName);

            currentClaims.Add(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
            currentClaims.Add(new(ClaimTypes.GivenName, user.UserName));
            currentClaims.Add(new(ClaimTypes.Role, user.Role?.Name ?? string.Empty));
            currentClaims.Add(new(ClaimTypes.AuthenticationMethod, scheme));
            return currentClaims;
        }
    }
}
