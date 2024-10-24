using FormsCreator.Core.Models.Base;
using FormsCreator.Core.Shared;
using System;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Base
{
    /// <summary>
    /// Base repository that encapsulates all common methods for entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IRepositoryBase<in TEntity> where TEntity : EntityBase
    {
        /// <summary>
        /// Creates a(n) <typeparamref name="TEntity"/> entry in the database.
        /// </summary>
        /// <param name="entity">The entity to add in the database.</param>
        /// <returns>An operation result.</returns>
        Task<IResult<Guid>> CreateAsync(TEntity entity);
    }
}
