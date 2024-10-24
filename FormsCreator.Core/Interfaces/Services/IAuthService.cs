using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FormsCreator.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<IResult> SignInAsync(UserLoginRequestDto request, CancellationToken token);

        Task<IResult> SignInExternalAsync(string scheme);

        bool IsSignedIn();

        Task SignOutAsync();

        bool HasRole(string roleName);

        string GetUserName();

        Guid GetUserId();
    }
}
