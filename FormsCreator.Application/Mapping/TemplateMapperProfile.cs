using AutoMapper;
using FormsCreator.Core.DTOs.Template;
using FormsCreator.Core.Models;
using Microsoft.AspNetCore.Http;

namespace FormsCreator.Application.Mapping
{
    internal class TemplateMapperProfile : Profile
    {
        public TemplateMapperProfile()
        {
            CreateMap<Template, TemplateResponseDto>();

            CreateMap<TemplateCreateRequestDto<IFormFile>, Template>()
                .AfterMap((src, target) =>
                {
                    target.UsersAllowed = src.Users?.Select(x => new TemplateAccess { UserId = x.Id }).ToArray() ?? [];
                });

            CreateMap<TemplateUpdateRequestDto<IFormFile>, Template>();

            CreateMap<TemplateResponseDto, TemplateUpdateRequestDto<IFormFile>>();
        }
    }
}
