using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface ITopicRepository
    {
        Task<IResult<IEnumerable<Topic>>> GetAllAsync(int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<Topic>>> GetAllAsync(CancellationToken token = default);

        Task<IResult<long>> CountAllAsync(CancellationToken cancellationToken = default);
    }
}
