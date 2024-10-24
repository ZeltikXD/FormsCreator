using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IAnswerRepository : IRepositoryBase<Answer>
    {
        Task<IResult<IEnumerable<Answer>>> GetByFormAsync(Guid formId, CancellationToken token = default);

        Task<bool> AreFromFormAsync(Guid formId, IEnumerable<Guid> answersId, CancellationToken token = default);
    }
}
