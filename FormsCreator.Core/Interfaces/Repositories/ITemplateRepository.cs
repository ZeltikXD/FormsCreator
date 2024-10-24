using FormsCreator.Core.Interfaces.Base;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using FormsCreator.Core.Enums;

namespace FormsCreator.Core.Interfaces.Repositories
{
    public interface ITemplateRepository : IRepositoryBase<Template>
    {
        Task<IResult<IEnumerable<Template>>> GetMostPopularAsync(int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<Template>>> GetAllAsync(int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<Template>>> GetByTopicsAsync(int page, int size, string[] topics, CancellationToken token = default);

        Task<IResult<IEnumerable<Template>>> GetByTagsAsync(int page, int size, string[] tags, CancellationToken token = default);

        Task<IResult<IEnumerable<Template>>> GetByCreatorAsync(Guid userId, int page, int size, CancellationToken token = default);

        Task<IResult<IEnumerable<Template>>> GetByTextAsync(int page, int size, string text, SupportedLang lang, CancellationToken token = default);

        Task<IResult<Template>> FindAsync(Guid id, CancellationToken token = default);

        Task<IResult> UpdateAsync(Template template);

        Task<IResult> DeleteAsync(Guid id);

        Task<IResult<long>> CountAllAsync(CancellationToken token = default);

        Task<IResult<long>> CountByTopicsAsync(string[] topics, CancellationToken token = default);

        Task<IResult<long>> CountByTagsAsync(string[] tags, CancellationToken token = default);

        Task<IResult<long>> CountByCreatorAsync(Guid userId, CancellationToken token = default);

        Task<IResult<long>> CountByTextAsync(string text, SupportedLang lang, CancellationToken token = default);

        Task<IResult> HasFormsFilledOutAsync(Guid id, CancellationToken token = default);
    }
}
