using FormsCreator.Core.DTOs.Template;
using FormsCreator.Core.Enums;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface ITemplateService<TImage> where TImage : class
    {
        Task<IResult> CreateAsync(TemplateCreateRequestDto<TImage> template);

        Task<IResult<IEnumerable<TemplateResponseDto>>> GetMostPopularAsync(int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<TemplateResponseDto>>> GetAllAsync(int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<TemplateResponseDto>>> GetByTopicsAsync(int page, int size, string[] topics, CancellationToken token);

        Task<IResult<IEnumerable<TemplateResponseDto>>> GetByTagsAsync(int page, int size, string[] tags, CancellationToken token);

        Task<IResult<IEnumerable<TemplateResponseDto>>> GetByCreatorAsync(Guid userId, int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<TemplateResponseDto>>> GetByTextAsync(int page, int size, string text, SupportedLang lang, CancellationToken token);

        Task<IResult<TemplateResponseDto>> FindAsync(Guid id, CancellationToken token);

        Task<IResult<TemplateUpdateRequestDto<TImage>>> FindAsUpdateAsync(Guid id, CancellationToken token);

        Task<IResult> UpdateAsync(TemplateUpdateRequestDto<TImage> template);

        Task<IResult> DeleteAsync(Guid id);

        Task<IResult<long>> CountAllAsync(CancellationToken token);

        Task<IResult<long>> CountByTopicsAsync(string[] topics, CancellationToken token);

        Task<IResult<long>> CountByTagsAsync(string[] tags, CancellationToken token);

        Task<IResult<long>> CountByCreatorAsync(Guid userId, CancellationToken token);

        Task<IResult<long>> CountByTextAsync(string text, SupportedLang lang, CancellationToken token);
    }
}
