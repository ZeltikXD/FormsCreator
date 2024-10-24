using FormsCreator.Core.DTOs.Form;
using FormsCreator.Core.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface IFormService
    {
        Task<IResult> CreateAsync(FormAddRequestDto form);

        Task<IResult<IEnumerable<FormResponseDto>>> GetByTemplateAsync(Guid templateId, int page, int size, CancellationToken token);

        Task<IResult<IEnumerable<FormResponseDto>>> GetByUserAsync(Guid userId, int page, int size, CancellationToken token);

        Task<IResult<FormResponseDto>> FindAsync(Guid id, CancellationToken token);
        
        Task<IResult<FormUpdateRequestDto>> FindAsUpdateAsync(Guid id, CancellationToken token);

        Task<IResult<long>> CountByTemplateAsync(Guid templateId, CancellationToken token);

        Task<IResult<long>> CountByUserAsync(Guid userId, CancellationToken token);

        Task<IResult> UpdateAsync(FormUpdateRequestDto form);
    }
}
