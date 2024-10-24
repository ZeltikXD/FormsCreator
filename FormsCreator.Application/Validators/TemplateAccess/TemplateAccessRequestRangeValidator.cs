using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.TemplateAccess;

namespace FormsCreator.Application.Validators.TemplateAccess
{
    internal class TemplateAccessRequestRangeValidator : AbstractValidator<TemplateAccessRequestRangeDto>
    {
        public TemplateAccessRequestRangeValidator()
        {
            RuleFor(x => x.TemplateId).NotEmpty()
                .WithMessage(ValidationMessages.CommonTemplateId);

            RuleFor(x => x.UserIds).NotEmpty()
                .WithMessage(ValidationMessages.TemplateAccessRangeUserIdsNotEmpty);
        }
    }
}
