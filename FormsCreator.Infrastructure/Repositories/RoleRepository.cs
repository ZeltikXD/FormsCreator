using FormsCreator.Core.Interfaces.Repositories;
using FormsCreator.Core.Models;
using FormsCreator.Core.Shared;
using FormsCreator.Infrastructure.Data;
using FormsCreator.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FormsCreator.Infrastructure.Repositories
{
    internal class RoleRepository(FormsDbContext context, ILogger<RoleRepository> logger)
        : RepositoryBase<Role>(context, logger), IRoleRepository
    {
        public Task<IResult<IEnumerable<Role>>> GetAllAsync(CancellationToken token = default)
            => ExecuteGetAsync(async () => await _context.Roles.ToListAsync(token));
    }
}
