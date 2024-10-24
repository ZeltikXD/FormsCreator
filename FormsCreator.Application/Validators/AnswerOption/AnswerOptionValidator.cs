using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.AnswerOption;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Validators.AnswerOption
{
    internal class AnswerOptionValidator : AbstractValidator<AnswerOptionDto>
    {
        internal static readonly AnswerOptionValidator _instance = new();

        public AnswerOptionValidator()
        {
            RuleFor(x => x.Value).MaximumLength(Constraints.MAX_LENGTH_VALUE)
                .WithMessage(ValidationMessages.AnswerValueMax);

            RuleFor(x => x.Row).MaximumLength(Constraints.MAX_LENGTH_ROW_COLUMN)
                .WithMessage(ValidationMessages.AnswerRowMax);

            RuleFor(x => x.Column).MaximumLength(Constraints.MAX_LENGTH_ROW_COLUMN)
                .WithMessage(ValidationMessages.AnswerColumnMax);

            RuleFor(x => x.Value).NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Row) && string.IsNullOrWhiteSpace(x.Column) && x.QuestionOptionId == default)
                .WithMessage(ValidationMessages.AnswerValueNotEmpty);

            RuleFor(x => x.Row).NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Value) && string.IsNullOrWhiteSpace(x.Column) && x.QuestionOptionId == default)
                .WithMessage(ValidationMessages.AnswerRowNotEmpty);

            RuleFor(x => x.Column).NotEmpty()
                .When(x => string.IsNullOrWhiteSpace(x.Value) && string.IsNullOrWhiteSpace(x.Row) && x.QuestionOptionId == default)
                .WithMessage(ValidationMessages.AnswerColumnNotEmpty);
        }
    }
}
