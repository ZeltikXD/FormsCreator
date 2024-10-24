using AutoMapper;
using FormsCreator.Core.DTOs.QuestionOption;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class QuestionOptionMapperProfile : Profile
    {
        public QuestionOptionMapperProfile()
        {
            CreateMap<QuestionOption, QuestionOptionResponseDto>();

            CreateMap<QuestionOptionRequestDto, QuestionOption>();

            CreateMap<QuestionOptionResponseDto, QuestionOptionRequestDto>();
        }
    }
}
