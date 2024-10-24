using FormsCreator.Core.Models.Base;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents user role.
    /// </summary>
    public class Role : EntityBase
    {
        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        public string Name { get; set; } = string.Empty;


        public ICollection<User> Users { get; set; } = null!;
    }
}
