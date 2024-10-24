using FormsCreator.Core.DTOs.TemplateAccess;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface ITemplateAccessService
    {
        Task<IResult> AddAsync(TemplateAccessRequestDto req);

        Task<IResult> AddRangeAsync(IEnumerable<Guid> userids, Guid templateId);

        Task<IResult> DeleteAsync(Guid userId, Guid templateId);

        Task<IResult> DeleteRangeAsync(IEnumerable<Guid> userids, Guid templateId);
    }
}
