using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Abstractions
{
    public interface ISalesforceManager
    {
        Task<IResult<string>> CreateAccountAsync(User user);

        Task<IResult> CreateContactAsync(User user, string accountId);
    }
}
