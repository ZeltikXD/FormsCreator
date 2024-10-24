using System;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Intermediate table between users and templates but specifically to know if a user has access to a template.
    /// </summary>
    public class TemplateAccess
    {
        /// <summary>
        /// Gets or sets the ID of the target template.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user that is authorize to access to the target template.
        /// </summary>
        public Guid UserId { get; set; }

        public Template Template { get; set; } = null!;

        public User User { get; set; } = null!;
    }
}
