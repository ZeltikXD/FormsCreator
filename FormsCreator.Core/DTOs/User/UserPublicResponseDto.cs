using FormsCreator.Core.DTOs.Role;
using System;

namespace FormsCreator.Core.DTOs.User
{
    public class UserPublicResponseDto
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string UserName { get; set; } = string.Empty;

        public bool IsBlocked { get; set; }

        public RoleResponseDto Role { get; set; } = null!;
    }
}
