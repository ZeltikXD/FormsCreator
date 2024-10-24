using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Application.Validators.Answer;
using FormsCreator.Core.DTOs.Form;

namespace FormsCreator.Application.Validators.Form
{
    internal class FormUpdateRequestValidator : AbstractValidator<FormUpdateRequestDto>
    {
        public FormUpdateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .WithMessage(ValidationMessages.FormId);

            RuleFor(x => x.Answers).NotEmpty()
                .WithMessage(ValidationMessages.FormAnswersNotEmpty);

            RuleForEach(x => x.Answers).SetValidator(AnswerRequestValidator._answerValidator);
        }
    }
}
