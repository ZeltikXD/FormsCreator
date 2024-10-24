using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Application.Validators.AnswerOption;
using FormsCreator.Core.DTOs.Answer;

namespace FormsCreator.Application.Validators.Answer
{
    internal class AnswerRequestValidator : AbstractValidator<AnswerRequestDto>
    {
        internal static readonly AnswerRequestValidator _answerValidator = new();

        public AnswerRequestValidator()
        {
            RuleFor(x => x.QuestionId).NotEmpty()
                .WithMessage(ValidationMessages.AnswerQuestionId);

            RuleFor(x => x.FormId).Empty()
                .WithMessage(ValidationMessages.AnswerFormId);

            RuleFor(x => x.Options).NotEmpty();

            RuleForEach(x => x.Options).SetValidator(AnswerOptionValidator._instance)
                .When(x => x.Options is not null);
        }
    }
}
