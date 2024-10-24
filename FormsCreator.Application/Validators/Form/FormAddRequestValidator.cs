using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Application.Validators.Answer;
using FormsCreator.Core.DTOs.Form;

namespace FormsCreator.Application.Validators.Form
{
    internal class FormAddRequestValidator : AbstractValidator<FormAddRequestDto>
    {
        public FormAddRequestValidator()
        {
            RuleFor(x => x.TemplateId).NotEmpty()
                .WithMessage(ValidationMessages.CommonTemplateId);

            RuleFor(x => x.Answers).NotEmpty()
                .WithMessage(ValidationMessages.FormAnswersNotEmpty);

            RuleFor(x => x.TotalQuestions).Equal(x => x.Answers.Count)
                .WithMessage("All questions must be answered.");

            RuleForEach(x => x.Answers).SetValidator(AnswerRequestValidator._answerValidator);
        }
    }
}
