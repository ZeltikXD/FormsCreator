using FormsCreator.Core.Models.Base;
using System;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a comment made by a user on a specific template.
    /// Comments are typically associated with templates and can be viewed by other users 
    /// depending on the access rights of the template.
    /// </summary>
    public class Comment : EntityUpdate
    {
        /// <summary>
        /// Gets or sets the ID of the user who posted the comment.
        /// This property associates the comment with the user who authored it.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the template on which the comment was made.
        /// This links the comment to the specific template.
        /// </summary>
        public Guid TemplateId { get; set; }

        /// <summary>
        /// Gets or sets the content of the comment.
        /// This is the actual text or message that the user has written.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user that belongs this comment. This navigational property links to the user entity.
        /// </summary>
        public User User { get; set; } = null!;

        /// <summary>
        /// Gets or sets the template that belongs this comment. This navigational property links to the template entity.
        /// </summary>
        public Template Template { get; set; } = null!;
    }
}
