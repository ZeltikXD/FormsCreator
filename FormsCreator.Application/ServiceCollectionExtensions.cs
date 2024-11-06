using FluentValidation;
using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Options;
using FormsCreator.Application.Services;
using FormsCreator.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FormsCreator.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            return services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly, includeInternalTypes: true)
                .AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly).AddHttpContextAccessor()
                .AddScoped<ICommentService, CommentService>().AddScoped<IFormService, FormService>()
                .AddScoped<IRoleService, RoleService>().AddScoped<ITemplateAccessService, TemplateAccessService>()
                .AddScoped<ITemplateService, TemplateService>().AddScoped<ITopicService, TopicService>()
                .AddScoped<IUserService, UserService>().AddScoped<ITagService, TagService>()
                .AddScoped<ISalesforceService, SalesforceService>().AddSingleton<ICommentNotifier, CommentNotifier>()
                .AddScoped<IJiraService, JiraService>();
        }

        public static IServiceCollection AddJwtBearerAuth(this IServiceCollection services, Action<TokenOptions> options, IWebHostEnvironment env)
        {
            TokenOptions opts = new();
            options(opts);
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = env.IsProduction();
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = env.IsProduction(),
                    ValidateAudience = env.IsProduction(),
                    ValidAudience = opts.Audience,
                    ValidIssuer = opts.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(opts.SecretKey))
                };
            });
            services.AddOptions<TokenOptions>().Configure(options);
            return services.AddScoped<IAuthService, AuthService>();
        }

        public static IServiceCollection AddGoogleOAuth(this IServiceCollection services, Action<GoogleOptions> options)
        {
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder = defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT_OR_COOKIE";
                options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            }).AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string? authorization = context.Request.Headers.Authorization;
                    if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                        return JwtBearerDefaults.AuthenticationScheme;

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            })
            .AddCookie()
            .AddGoogle(x =>
            {
                options(x);
                x.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            return services;
        }

        public static IServiceCollection AddEmailSender(this IServiceCollection services, Action<EmailSenderOptions> options)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            services.AddSingleton<IEmailSender, EmailSender>()
                .AddOptions<EmailSenderOptions>().Configure(options);
            return services;
        }
    }
}
