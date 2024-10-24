using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.Tag;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Validators.Tag
{
    internal class TagValidator : AbstractValidator<TagDto>
    {
        internal static readonly TagValidator _tagValidator = new();

        public TagValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage(ValidationMessages.TagNameNotEmpty)
                .MaximumLength(Constraints.MAX_LENGTH_TAG_NAME)
                .WithMessage(ValidationMessages.TagNameMax)
                .When(x => x.Id == default);
        }
    }
}
