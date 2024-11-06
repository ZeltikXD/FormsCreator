using FormsCreator.Application.Abstractions;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class SalesforceService(IUserRepository userRepository,
        ISalesforceManager salesforceManager) : ISalesforceService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ISalesforceManager _salesforceManager = salesforceManager;

        public async Task<IResult> CreateAccountAndContactAsync(Guid userId)
        {
            var userRes = await _userRepository.FindByIdAsync(userId);
            if (userRes.IsFailure) return userRes;
            var accResult = await _salesforceManager.CreateAccountAsync(userRes.Result);
            if (accResult.IsFailure) return accResult;
            return await _salesforceManager.CreateContactAsync(userRes.Result, accResult.Result);
        }
    }
}
