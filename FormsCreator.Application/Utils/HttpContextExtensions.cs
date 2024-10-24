using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FormsCreator.Application.Utils
{
    public static class HttpContextExtensions
    {
        public static Guid GetCurrentUserId(this HttpContext? httpContext)
        {
            if (httpContext is null) return default;
            var id = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(id, out var guid) ? guid : default;
        }

        public static string GetCurrrentUserName(this HttpContext? context)
        {
            if (context is null) return string.Empty;
            return context.User.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
        }
    }
}
