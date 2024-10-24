using System;

namespace FormsCreator.Core.Models.Base
{
    /// <summary>
    /// Base entity for entities that track the basic update.
    /// </summary>
    public abstract class EntityUpdate : EntityBase
    {
        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        public virtual DateTimeOffset UpdatedAt { get; set; }
    }
}
