using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.Like;

namespace FormsCreator.Application.Validators.Like
{
    internal class LikeRequestValidator : AbstractValidator<LikeRequestDto>
    {
        public LikeRequestValidator()
        {
            RuleFor(x => x.TemplateId).NotEmpty()
                .WithMessage(ValidationMessages.CommonTemplateId);
        }
    }
}
