using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Hosting;

namespace FormsCreator.Application.Utils
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJwtBearerAuth(this IApplicationBuilder app)
            => app.Use(async (context, next) =>
            {
                if (context.Request.Cookies.TryGetValue("session_token", out var token) && !string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Append("Authorization", string.Format("Bearer {0}", token));
                }
                await next(context);
            });

        public static IApplicationBuilder UseAuthRedirection(this IApplicationBuilder app)
            => app.UseStatusCodePages(context => Task.Run(() => {
                var request = context.HttpContext.Request;
                var response = context.HttpContext.Response;

                if (response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    var returnUrl = request.Method == "POST" ? request.GetTypedHeaders().Referer?.PathAndQuery ?? string.Empty
                        : request.Path + request.QueryString;
                    response.Redirect($"/auth/login?returnUrl={returnUrl}");
                }
            }));

        public static IApplicationBuilder UseForwardedHeaders(this WebApplication app)
        {
            if (app.Environment.IsProduction())
            {
                app.UseForwardedHeaders(new()
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                    | ForwardedHeaders.XForwardedHost
                });
            }
            return app;
        }

        public static IApplicationBuilder UseChangeLanguage(this IApplicationBuilder app)
            => app.Use(async (context, next) =>
            {
                var lang = context.Request.GetUserLanguage();
                GenericUtils.SetLanguage(lang);

                await next(context);
            });
    }
}
