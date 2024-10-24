using AutoMapper;
using FormsCreator.Core.DTOs.Answer;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class AnswerMapperProfile : Profile
    {
        public AnswerMapperProfile()
        {
            CreateMap<Answer, AnswerResponseDto>();

            CreateMap<AnswerRequestDto, Answer>();

            CreateMap<Answer, AnswerRequestDto>();
        }
    }
}
