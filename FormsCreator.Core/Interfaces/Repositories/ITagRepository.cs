using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
        Task<IResult<Tag>> FindAsync(string tagName, CancellationToken token = default);

        Task<IResult<Tag>> FindAsync(Guid id, CancellationToken token = default);

        Task<IResult<IEnumerable<Tag>>> GetAsync(CancellationToken token = default);

        Task<IResult<IEnumerable<Tag>>> GetAsync(int size, CancellationToken token = default);
    }
}
