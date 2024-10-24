using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Repositories
{
	public interface IAnswerOptionRepository
	{
		Task<IResult<IEnumerable<AnswerOption>>> GetByAnswerAsync(Guid answerId, CancellationToken token = default);

		Task<IResult> UpdateRangeAsync(IEnumerable<AnswerOption> answerOptions);
	}
}