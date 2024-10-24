using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IUserProviderRepository : IRepositoryBase<UserProvider>
    {
        /// <summary>
        /// This method get the user providers of an specific user.
        /// </summary>
        /// <param name="userId">The user that has the providers.</param>
        /// <param name="token">Cancellation task token.</param>
        /// <returns>An operation result with the user providers.</returns>
        Task<IResult<IEnumerable<UserProvider>>> GetProvidersAsync(Guid userId, CancellationToken token = default);

        Task<IResult> UpdateAsync(UserProvider provider);

        Task<IResult> DeleteAsync(Guid id);
    }
}
