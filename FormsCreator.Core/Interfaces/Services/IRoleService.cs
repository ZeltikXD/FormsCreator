using FormsCreator.Core.DTOs.Role;
using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IResult<IEnumerable<RoleResponseDto>>> GetAllAsync(CancellationToken token);
    }
}
