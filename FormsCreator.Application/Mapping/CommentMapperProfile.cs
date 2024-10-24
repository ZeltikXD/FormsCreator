using AutoMapper;
using FormsCreator.Core.DTOs.Comment;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
    internal class CommentMapperProfile : Profile
    {
        public CommentMapperProfile() 
        {
            CreateMap<Comment, CommentResponseDto>();

            CreateMap<CommentRequestDto, Comment>();
        }
    }
}
