using AutoMapper;
using FormsCreator.Core.DTOs.Topic;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class TopicMapperProfile : Profile
    {
        public TopicMapperProfile()
        {
            CreateMap<Topic, TopicResponseDto>();
        }
    }
}
