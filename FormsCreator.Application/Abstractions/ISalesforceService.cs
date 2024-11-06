using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Abstractions
{
    public interface ISalesforceService
    {
        Task<IResult> CreateAccountAndContactAsync(Guid userId);
    }
}
