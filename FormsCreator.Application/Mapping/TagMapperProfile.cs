using AutoMapper;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.Models;

namespace FormsCreator.Application.Mapping
{
	internal class TagMapperProfile : Profile
	{
		public TagMapperProfile()
		{
			CreateMap<Tag, TagDto>();

			CreateMap<TagDto, Tag>();
		}
	}
}