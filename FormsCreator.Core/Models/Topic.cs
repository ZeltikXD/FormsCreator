using FormsCreator.Core.Models.Base;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a topic used to categorize or label templates or forms.
    /// Topics allow users to search and filter templates based on specific categories or labels.
    /// </summary>
    public class Topic : EntityBase
    {
        /// <summary>
        /// Gets or sets the name of the tag.
        /// The name is a unique label that helps categorize templates.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public ICollection<Template> Templates { get; set; } = null!;
    }
}
