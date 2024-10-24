using AutoMapper;
using FormsCreator.Core.DTOs.Form;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class FormMapperProfile : Profile
    {
        public FormMapperProfile()
        {
            CreateMap<Form, FormResponseDto>();

            CreateMap<FormAddRequestDto, Form>();

            CreateMap<Form, FormUpdateRequestDto>();
        }
    }
}
