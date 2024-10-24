using AutoMapper;
using FormsCreator.Core.DTOs.Like;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class LikeMapperProfile : Profile
    {
        public LikeMapperProfile()
        {
            CreateMap<LikeRequestDto, Like>();
        }
    }
}
