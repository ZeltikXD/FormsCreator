using Atlassian.Jira;
using EntityFramework.Exceptions.PostgreSQL;
using FormsCreator.Application.Abstractions;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Options;
using FormsCreator.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace FormsCreator.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PostgreSQL Database.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionStr"></param>
        /// <returns>A reference to the same <see cref="IServiceCollection"/> instance.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddPostgreDbContext(this IServiceCollection services, [NotNull] string? connectionStr)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            if (string.IsNullOrWhiteSpace(connectionStr)) throw new ArgumentException("The connection string cannot be empty.", nameof(connectionStr));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services.AddDbContext<FormsDbContext>(o => o.UseNpgsql(connectionStr).UseExceptionProcessor());
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));

            return services.AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
            => services.AddScoped<IAnswerRepository, AnswerRepository>()
            .AddScoped<ICommentRepository, CommentRepository>().AddScoped<IFormRepository, FormRepository>()
            .AddScoped<ILikeRepository, LikeRepository>().AddScoped<IQuestionOptionRepository, QuestionOptionRepository>()
            .AddScoped<IQuestionRepository, QuestionRepository>().AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ITemplateAccessRepository, TemplateAccessRepository>().AddScoped<ITemplateRepository, TemplateRepository>()
            .AddScoped<ITopicRepository, TopicRepository>().AddScoped<IUserProviderRepository, UserProviderRepository>()
            .AddScoped<IUserRepository, UserRepository>().AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();

        public static IServiceCollection AddImageManager(this IServiceCollection services, Action<ImageManagerOptions> opts)
        {
            services.AddOptions<ImageManagerOptions>().Configure(opts);
            services.AddHttpClient<IImageManager, ImageManager>();
            return services;
        }

        public static IServiceCollection AddSalesforceManager(this IServiceCollection services, Action<SalesforceOptions> opts)
        {
            services.AddOptions<SalesforceOptions>().Configure(opts);
            services.AddHttpClient<ISalesforceManager, SalesforceManager>();
            return services;
        }

        public static IServiceCollection AddJiraManager(this IServiceCollection services, Action<AtlassianJiraOptions> opts)
        {
            services.AddOptions<AtlassianJiraOptions>().Configure(opts);
            return services.AddSingleton<IAtlassianJiraManager, AtlassianJiraManager>(ctr =>
            {
                var options = ctr.GetRequiredService<IOptions<AtlassianJiraOptions>>();
                var jira = Jira.CreateRestClient(options.Value.ApiUrl, options.Value.EmailAccount, options.Value.ApiToken,
                    new() { EnableRequestTrace = true, EnableUserPrivacyMode = true });
                return new AtlassianJiraManager(jira, options);
            });
        }
    }
}
