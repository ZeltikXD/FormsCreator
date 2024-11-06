using Atlassian.Jira;
using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Utils;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Options;
using FormsCreator.Infrastructure.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using JiraUserCreationInfo = FormsCreator.Application.Records.JiraUserCreationInfo;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class AtlassianJiraManager(Jira jira,
        IOptions<AtlassianJiraOptions> options) : IAtlassianJiraManager
    {
        private readonly Jira _jira = jira;
        private readonly AtlassianJiraOptions _options = options.Value;
        const string DefaultDescription = "A ticket made by an user from the FormCraft app. {0}";

        public async Task<IResult<object>> CreateIssueAsync(Priority priority, string? templateTitle, string summary, string link, string reportedBy)
        {
            try
            {
                var issue = _jira.CreateIssue(_options.ProjectName);
                issue.Summary = summary;
                issue.Description = GetWithTemplateTitle(templateTitle);
                issue.Priority = new(((byte)priority).ToString());
                issue.Reporter = reportedBy;
                issue.Type = new("10002");
                issue = await issue.SaveChangesAsync();
                return Result.Success<object>(issue);
            }
            catch (Exception ex)
            {
                return Result.Failure<object>(new(ResultErrorType.UnknownError, ex.Message));
            }
        }

        public async Task<IResult<JiraUserCreationInfo>> CreateUserAsync(User user)
        {
            try
            {
                var (exists, id) = await UserExistsAsync(user.Email);
                if (exists) return Success(new(id, false, string.Empty));
                var jiraUser = PrepareUserInfo(user);
                var createdUser = await _jira.Users.CreateUserAsync(jiraUser);
                return Success(new(createdUser.AccountId, true, jiraUser.Password));
            }
            catch (Exception ex)
            {
                return Failure(new(ResultErrorType.UnknownError, ex.Message));
            }
        }

        private static string GetWithTemplateTitle(string? title)
            => string.Format(DefaultDescription, string.IsNullOrWhiteSpace(title) ? string.Empty : $"Template: {title}");

        private async Task<(bool Exists, string Id)> UserExistsAsync(string email)
        {
            var user = (await _jira.Users.SearchUsersAsync(email)).FirstOrDefault();
            return (user != null, user?.AccountId ?? string.Empty);
        }

        private static JiraUserCreationInfoEnhanced PrepareUserInfo(User user)
            => new()
            {
                DisplayName = user.UserName,
                Email = user.Email,
                Username = user.UserName,
                Password = HashUtils.HashPassword(user.UserName + user.Email).password,
                Notification = false,
                Products = [ "jira-software" ]
            };

        static IResult<JiraUserCreationInfo> Success(JiraUserCreationInfo res)
            => Result.Success(res);

        static IResult<JiraUserCreationInfo> Failure(ErrorResult error)
            => Result.Failure<JiraUserCreationInfo>(error);

        class JiraUserCreationInfoEnhanced : Atlassian.Jira.JiraUserCreationInfo
        {
            [JsonProperty("products")]
            public IList<string> Products { get; set; } = null!;
        }
    }
}
