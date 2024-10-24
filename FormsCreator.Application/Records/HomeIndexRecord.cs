using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.DTOs.Template;

namespace FormsCreator.Application.Records
{
    public record HomeIndexRecord(IEnumerable<TemplateResponseDto> MostPopular,
        IEnumerable<TemplateResponseDto> MostRecent, IEnumerable<TagDto> Tags);
}
