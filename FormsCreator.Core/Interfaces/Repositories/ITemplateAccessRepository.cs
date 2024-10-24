using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface ITemplateAccessRepository
    {
        Task<IResult> CreateAsync(TemplateAccess access);

        Task<IResult> CreateRangeAsync(IEnumerable<TemplateAccess> access);

        Task<IResult> DeleteAsync(Guid userId, Guid templateId);

        Task<IResult> DeleteRangeAsync(IEnumerable<TemplateAccess> access);

        Task<IResult<bool>> UserHasPermissionAsync(Guid userId, Guid templateId, CancellationToken token = default);
    }
}
