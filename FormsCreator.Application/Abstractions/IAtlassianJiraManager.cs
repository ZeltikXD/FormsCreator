using FormsCreator.Application.Records;
using FormsCreator.Application.Utils;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Abstractions
{
	public interface IAtlassianJiraManager
	{
		Task<IResult<JiraUserCreationInfo>> CreateUserAsync(User user);

		Task<IResult<object>> CreateIssueAsync(Priority priority, string? templateTitle, string summary, string link, string reportedBy);
	}
}