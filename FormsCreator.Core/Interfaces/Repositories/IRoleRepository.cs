using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<IResult<IEnumerable<Role>>> GetAllAsync(CancellationToken token = default);
    }
}
