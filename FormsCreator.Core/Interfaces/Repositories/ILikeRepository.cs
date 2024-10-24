using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface ILikeRepository
    {
        Task<IResult> CreateAsync(Like like);

        Task<IResult> UpdateAsync(Like like);

        Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token = default);
    }
}
