using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface IQuestionOptionRepository : IRepositoryBase<QuestionOption>
    {
        Task<IResult<IEnumerable<QuestionOption>>> GetByQuestionAsync(Guid questionId, CancellationToken token = default);

        Task<IResult> UpdateAsync(QuestionOption questionOption);

        Task<IResult> UpdateRangeAsync(IEnumerable<QuestionOption> questionOptions);

        Task<IResult> DeleteAsync(Guid id);
    }
}
