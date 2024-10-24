using AutoMapper;
using FormsCreator.Core.DTOs.Question;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class QuestionMapperProfile : Profile
    {
        public QuestionMapperProfile()
        {
            CreateMap<Question, QuestionResponseDto>();

            CreateMap<QuestionRequestDto, Question>();

            CreateMap<QuestionResponseDto, QuestionRequestDto>();
        }
    }
}
