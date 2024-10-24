using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IQuestionRepository : IRepositoryBase<Question>
    {
        Task<IResult<IEnumerable<Question>>> GetByTemplateAsync(Guid templateId, CancellationToken token = default);

        Task<IResult> UpdateAsync(Question question);

        Task<IResult> UpdateRangeAsync(IEnumerable<Question> questions);

        Task<IResult> DeleteAsync(Guid id);
    }
}
