using System;

namespace FormsCreator.Core.DTOs.UserProvider
{
    public class UserProviderResponseDto
    {
        public Guid Id { get; set; }

        public string Provider { get; set; } = string.Empty;
    }
}
