using AutoMapper;
using FormsCreator.Core.DTOs.Role;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class RoleMapperProfile : Profile
    {
        public RoleMapperProfile()
        {
            CreateMap<Role, RoleResponseDto>();
        }
    }
}
