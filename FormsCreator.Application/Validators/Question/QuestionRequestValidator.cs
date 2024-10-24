using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Application.Validators.QuestionOption;
using FormsCreator.Core.DTOs.Question;
using FormsCreator.Core.Shared;

namespace FormsCreator.Application.Validators.Question
{
    internal class QuestionRequestValidator : AbstractValidator<QuestionRequestDto>
    {
        internal static readonly QuestionRequestValidator _questionValidator = new();

        public QuestionRequestValidator()
        {
            RuleFor(x => x.Text).NotEmpty()
                .WithMessage(ValidationMessages.QuestionTextNotEmpty);

            RuleFor(x => x.Text).MaximumLength(Constraints.MAX_LENGTH_QUESTION_TEXT)
                .WithMessage(ValidationMessages.QuestionTextMax);

            RuleFor(x => x.Description).MaximumLength(Constraints.MAX_LENGTH_QUESTION_DESC)
                .WithMessage(ValidationMessages.QuestionDescMax);

            RuleFor(x => x.Type).IsInEnum()
                .WithMessage(ValidationMessages.QuestionType);

            RuleForEach(x => x.Options).SetValidator(QuestionOptionRequestValidator._optionValidator)
                .When(x => x.Options is not null && x.Options.Count != 0);
        }
    }
}
