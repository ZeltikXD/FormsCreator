using System;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a "like" given by a user to a specific template or form.
    /// A "like" allows users to express their approval or interest in a template.
    /// Each like is tied to a user and a template.
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Gets or sets the ID of the user who liked the template.
        /// This associates the like with the user who performed the action.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the template that was liked.
        /// This links the like to a specific template or form.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the like has been deleted.
        /// A deleted like means the user has removed their "like" from the template.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the user who reacts the target template. This navigational property links to the user entity.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the target template that the user reacted. This navigational property links to the template entity.
        /// </summary>
        public Template Template { get; set; } = null!;
    }
}
