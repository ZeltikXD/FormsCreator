using AutoMapper;
using FormsCreator.Core.DTOs.AnswerOption;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class AnswerOptionMapperProfile : Profile
    {
        public AnswerOptionMapperProfile()
        {
            CreateMap<AnswerOption, AnswerOptionDto>();

            CreateMap<AnswerOptionDto, AnswerOption>();
        }
    }
}
