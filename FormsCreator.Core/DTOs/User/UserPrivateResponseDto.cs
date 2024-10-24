using FormsCreator.Core.DTOs.UserProvider;
using System.Collections.Generic;

namespace FormsCreator.Core.DTOs.User
{
    public class UserPrivateResponseDto : UserPublicResponseDto
    {
        public string Email { get; set; } = string.Empty;

        public ICollection<UserProviderResponseDto> Providers { get; set; } = null!;
    }
}
