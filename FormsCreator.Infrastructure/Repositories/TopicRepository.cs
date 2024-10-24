using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class TopicRepository(FormsDbContext context, ILogger<TopicRepository> logger)
        : RepositoryBase<Topic>(context, logger), ITopicRepository
    {
        public Task<IResult<long>> CountAllAsync(CancellationToken cancellationToken = default)
            => ExecuteCountAsync(() => _context.Topics.LongCountAsync(cancellationToken));

        public Task<IResult<IEnumerable<Topic>>> GetAllAsync(int page, int size, CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Topics.OrderBy(x => x.Name)
                .Skip((page - 1) * size).Take(size).ToListAsync(token));

        public Task<IResult<IEnumerable<Topic>>> GetAllAsync(CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Topics.OrderBy(x => x.Name).ToListAsync(token));
    }
}
