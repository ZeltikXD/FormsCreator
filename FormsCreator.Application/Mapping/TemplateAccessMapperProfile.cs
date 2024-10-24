using AutoMapper;
using FormsCreator.Core.DTOs.TemplateAccess;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class TemplateAccessMapperProfile : Profile
    {
        public TemplateAccessMapperProfile()
        {
            CreateMap<TemplateAccessRequestDto, TemplateAccess>();
        }
    }
}
