using AutoMapper;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class TagService(ITagRepository repository, IMapper mapper) : ITagService
    {
        private readonly ITagRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IResult<IEnumerable<TagDto>>> GetAsync(CancellationToken token)
        {
            var result = await _repository.GetAsync(token);
            return MapToDto(result);
        }

        public async Task<IResult<IEnumerable<TagDto>>> GetAsync(int size, CancellationToken token)
        {
            var result = await _repository.GetAsync(size, token);
            return MapToDto(result);
        }

        public Task<IResult<Guid>> AddAsync(TagDto tagDto)
        {
            var tag = _mapper.Map<TagDto, Tag>(tagDto);
            return _repository.CreateAsync(tag);
        }

        IResult<IEnumerable<TagDto>> MapToDto(IResult<IEnumerable<Tag>> result)
        {
            if (result.IsFailure) return result.FailureTo<IEnumerable<TagDto>>();
            var tags = _mapper.Map<IEnumerable<Tag>, IEnumerable<TagDto>>(result.Result);
            return Result.Success(tags);
        }
    }
}
