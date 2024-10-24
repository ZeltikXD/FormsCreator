using System;

namespace FormsCreator.Core.Models.Base
{
    /// <summary>
    /// Base entity of all entities. Contains the base info.
    /// </summary>
    public abstract class EntityBase
    {
        /// <summary>
        /// Gets or sets the ID of the current entity.
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public virtual DateTimeOffset CreatedAt { get; set; }
    }
}