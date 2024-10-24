using FormsCreator.Core.Models.Base;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Allow users to invent their own “categorization” and evolve it over time.
    /// </summary>
    public class Tag : EntityBase
    {
        /// <summary>
        /// Gets or sets the name of the tag.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public ICollection<Template> Templates { get; set; } = null!;
    }
}
