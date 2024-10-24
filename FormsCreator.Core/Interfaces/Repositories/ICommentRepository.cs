using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Task<IResult<IEnumerable<Comment>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token = default);

        Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token = default);
    }
}
