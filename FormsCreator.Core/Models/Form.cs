using FormsCreator.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a filled-out form based on a specific template. A form contains the user's answers to the questions defined in the template.
    /// </summary>
    public class Form : EntityUpdate
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user who filled out the form.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the template on which the form is based.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the user who filled out the form. This navigational property links to the user entity.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the template used to create the form. This navigational property links to the template entity.
        /// </summary>
        public Template Template { get; set; } = null!;

        public ICollection<Answer> Answers { get; set; } = null!;
    }
}
