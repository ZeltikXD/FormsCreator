using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.Comment;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Validators.Comment
{
    internal class CommentRequestValidator : AbstractValidator<CommentRequestDto>
    {
        public CommentRequestValidator()
        {
            RuleFor(x => x.Content).NotEmpty()
                .WithMessage(ValidationMessages.CommentContentNotEmpty);

            RuleFor(x => x.Content).MaximumLength(Constraints.MAX_LENGTH_COMMENT_CONTENT)
                .WithMessage(ValidationMessages.CommentContentMax);

            RuleFor(x => x.TemplateId).NotEmpty()
                .WithMessage(ValidationMessages.CommonTemplateId);
        }
    }
}
