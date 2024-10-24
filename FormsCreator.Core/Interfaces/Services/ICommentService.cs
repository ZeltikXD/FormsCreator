using FormsCreator.Core.DTOs.Comment;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface ICommentService
    {
        Task<IResult<Guid>> AddAsync(CommentRequestDto comment);

        Task<IResult<IEnumerable<CommentResponseDto>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token);

        Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token);
    }
}
