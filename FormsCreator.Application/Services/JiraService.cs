using FormsCreator.Application.Abstractions;
using FormsCreator.Application.Records;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class JiraService(IUserRepository repository,
        IAtlassianJiraManager jiraManager) : IJiraService
    {
        private readonly IUserRepository _userRepository = repository;
        private readonly IAtlassianJiraManager _jiraManager = jiraManager;

        public async Task<IResult<object>> CreateTicketAsync(Guid userId, JiraIssue issue)
        {
            var userRes = await _userRepository.FindByIdAsync(userId);
            if (userRes.IsFailure) return userRes.FailureTo<object>();
            var result = await _jiraManager.CreateUserAsync(userRes.Result);
            if (result.IsFailure) return result.FailureTo<object>();
            return await _jiraManager.CreateIssueAsync(issue.Priority, issue.TemplateTitle, issue.Summary, issue.Referer, result.Result.Id);
        }
    }
}
