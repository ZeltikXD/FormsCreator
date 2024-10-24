using AutoMapper;
using FormsCreator.Core.DTOs.UserProvider;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class UserProviderMapperProfile : Profile
    {
        public UserProviderMapperProfile()
        {
            CreateMap<UserProvider, UserProviderResponseDto>();
        }
    }
}
