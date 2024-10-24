using AutoMapper;
using FormsCreator.Core.DTOs.User;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserPublicResponseDto>();

            CreateMap<User, UserPrivateResponseDto>();

            CreateMap<UserRegisterRequestDto, User>()
                .AfterMap((req, user) =>
                {
                    user.PasswordHash = req.Password;
                });
        }
    }
}
