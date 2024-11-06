using FormsCreator.Application.Records;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Abstractions
{
    public interface IJiraService
    {
        Task<IResult<object>> CreateTicketAsync(Guid userId, JiraIssue issue);
    }
}
