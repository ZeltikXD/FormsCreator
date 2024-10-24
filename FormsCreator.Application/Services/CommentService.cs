using AutoMapper;
using FormsCreator.Core.DTOs.Comment;
using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Interfaces.Services;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Services
{
    internal class CommentService(ICommentRepository repository,
        IMapper mapper) : ICommentService
    {
        private readonly ICommentRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public Task<IResult<Guid>> AddAsync(CommentRequestDto comment)
        {
            var cmm = _mapper.Map<CommentRequestDto, Comment>(comment);
            return _repository.CreateAsync(cmm);
        }

        public Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token)
        {
            return _repository.CountByTemplateAsync(templateId, token);
        }

        public async Task<IResult<IEnumerable<CommentResponseDto>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token)
        {
            var comments = await _repository.GetByTemplateAsync(templateId, page, size, token);
            if (comments.IsFailure) return comments.FailureTo<IEnumerable<CommentResponseDto>>();
            var cmms = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentResponseDto>>(comments.Result);
            return Result.Success(cmms);
        }
    }
}
