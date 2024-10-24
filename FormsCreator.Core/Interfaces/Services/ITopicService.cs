using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FormsCreator.Core.DTOs.Topic;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface ITopicService
    {
        Task<IResult<IEnumerable<TopicResponseDto>>> GetAllAsync(int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<TopicResponseDto>>> GetAllAsync(CancellationToken token);

        Task<IResult<long>> CountAllAsync(CancellationToken token);
    }
}
