using FormsCreator.Core.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FormsCreator.Core.DTOs.Tag;
using System;

namespace FormsCreator.Core.Interfaces.Services
{
	public interface ITagService
	{
        Task<IResult<IEnumerable<TagDto>>> GetAsync(CancellationToken token);

        Task<IResult<IEnumerable<TagDto>>> GetAsync(int size, CancellationToken token);

        Task<IResult<Guid>> AddAsync(TagDto tagDto);
    }
}