using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.QuestionOption;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Validators.QuestionOption
{
    internal class QuestionOptionRequestValidator : AbstractValidator<QuestionOptionRequestDto>
    {
        internal static readonly QuestionOptionRequestValidator _optionValidator = new();

        public QuestionOptionRequestValidator()
        {
            RuleFor(x => x.Value).NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Row) && string.IsNullOrWhiteSpace(x.Column))
                .WithMessage(ValidationMessages.QuestionOptionValueNotEmpty);
           
            RuleFor(x => x.Value).MaximumLength(Constraints.MAX_LENGTH_VALUE)
                .WithMessage(ValidationMessages.QuestionOptionValueMax);

            RuleFor(x => x.Row).MaximumLength(Constraints.MAX_LENGTH_ROW_COLUMN)
                .WithMessage(ValidationMessages.AnswerRowMax);

            RuleFor(x => x.Row).NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Column) && string.IsNullOrWhiteSpace(x.Value))
                .WithMessage(ValidationMessages.QuestionOptionRowNotEmpty);

            RuleFor(x => x.Column).NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Row) && string.IsNullOrWhiteSpace(x.Value))
                .WithMessage(ValidationMessages.QuestionOptionColumnNotEmpty);

            RuleFor(x => x.Column).MaximumLength(Constraints.MAX_LENGTH_ROW_COLUMN)
                .WithMessage(ValidationMessages.AnswerColumnMax);
        }
    }
}
