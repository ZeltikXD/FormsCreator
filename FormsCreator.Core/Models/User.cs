using FormsCreator.Core.Models.Base;
using System;
using System.Collections.Generic;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Represents a user in the system with associated authentication and authorization details.
    /// </summary>
    public class User : EntityBase
    {
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the hashed password of the user. This is stored securely to ensure the user's password is not exposed.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the salt used for hashing the user's password. This is used to enhance password security.
        /// </summary>
        public string PasswordSalt { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role identifier that indicates the user's role in the system (e.g., admin, user).
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is blocked. If true, the user cannot log in.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wether the user has a valid email. If false, the user cannot log in.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        public Role? Role { get; set; }


        public ICollection<Comment> Comments { get; set; } = null!;
        public ICollection<Form> Forms { get; set; } = null!;
        public ICollection<Like> Likes { get; set; } = null!;
        public ICollection<Template> Templates { get; set; } = null!;
        public ICollection<TemplateAccess> TemplateAccesses { get; set; } = null!;
        public ICollection<UserProvider> Providers { get; set; } = null!;
    }
}
