using FormsCreator.Core.Models.Base;
using System;

namespace FormsCreator.Core.Models
{
    /// <summary>
    /// Manages users and oauth methods to autenticate.
    /// </summary>
    public class UserProvider : EntityBase
    {
        /// <summary>
        /// Gets or sets the id of ther user that has provider accounts.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the provider's name.
        /// </summary>
        public string Provider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user provider id.
        /// </summary>
        public string ProviderId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets provider access token.
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the expiration time of the <see cref="AccessToken"/>.
        /// </summary>
        public DateTimeOffset ExpireTime { get; set; }

        public User User { get; set; } = null!;
    }
}
