using System;

namespace FormsCreator.Core.DTOs.User
{
    public class UserRequestDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;
    }
}
