using AutoMapper;
using FormsCreator.Core.DTOs.Topic;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class TopicService(ITopicRepository repository,
        IMapper mapper) : ITopicService
    {
        private readonly ITopicRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public Task<IResult<long>> CountAllAsync(CancellationToken token)
        {
            return _repository.CountAllAsync(token);
        }

        public async Task<IResult<IEnumerable<TopicResponseDto>>> GetAllAsync(int page, int size, CancellationToken token)
        {
            var topicsRes = await _repository.GetAllAsync(page, size, token);
            if (topicsRes.IsFailure) return topicsRes.FailureTo<IEnumerable<TopicResponseDto>>();
            var topics = _mapper.Map<IEnumerable<Topic>, IEnumerable<TopicResponseDto>>(topicsRes.Result);
            return Result.Success(topics);
        }

        public async Task<IResult<IEnumerable<TopicResponseDto>>> GetAllAsync(CancellationToken token)
        {
            var topicsRes = await _repository.GetAllAsync(token);
            if (topicsRes.IsFailure) return topicsRes.FailureTo<IEnumerable<TopicResponseDto>>();
            var topics = _mapper.Map<IEnumerable<Topic>, IEnumerable<TopicResponseDto>>(topicsRes.Result);
            return Result.Success(topics);
        }
    }
}
