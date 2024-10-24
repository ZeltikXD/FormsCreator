using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IFormRepository : IRepositoryBase<Form>
    {
        Task<IResult<IEnumerable<Form>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<Form>>> GetByUserAsync(Guid userId, int page, int size, CancellationToken token = default);

        Task<IResult<Form>> FindAsync(Guid id, CancellationToken token = default);

        Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token = default);

        Task<IResult<long>> CountByUserAsync(Guid userId, CancellationToken token = default);

        Task<IResult> ExistsAsync(Guid id, CancellationToken token = default);
    }
}
