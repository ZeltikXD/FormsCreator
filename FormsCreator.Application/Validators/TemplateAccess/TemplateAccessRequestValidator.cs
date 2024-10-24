using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.TemplateAccess;

namespace FormsCreator.Application.Validators.TemplateAccess
{
    internal class TemplateAccessRequestValidator : AbstractValidator<TemplateAccessRequestDto>
    {
        public TemplateAccessRequestValidator()
        {
            RuleFor(x => x.TemplateId).NotEmpty()
                .WithMessage(ValidationMessages.CommonTemplateId);
        }
    }
}
