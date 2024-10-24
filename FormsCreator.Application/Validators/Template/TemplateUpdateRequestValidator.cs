using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Application.Validators.Question;
using FormsCreator.Application.Validators.Tag;
using FormsCreator.Core.DTOs.Template;
using FormsCreator.Core.Shared;
using Microsoft.AspNetCore.Http;

namespace FormsCreator.Application.Validators.Template
{
    internal class TemplateUpdateRequestValidator : AbstractValidator<TemplateUpdateRequestDto<IFormFile>>
    {
        public TemplateUpdateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                .WithMessage(ValidationMessages.CommonTemplateId);

            RuleFor(x => x.Title).NotEmpty()
                .WithMessage(ValidationMessages.TemplateTitleNotEmpty)
                .MaximumLength(Constraints.MAX_LENGTH_TEMPL_TITLE)
                .WithMessage(ValidationMessages.TemplateTitleMax);

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage(ValidationMessages.TemplateDescNotEmpty)
                .MaximumLength(Constraints.MAX_LENGTH_TEMPL_DESC)
                .WithMessage(ValidationMessages.TemplateDescMax);

            RuleFor(x => x.Image).Must(x => x!.ContentType.Contains("image", StringComparison.InvariantCultureIgnoreCase))
                .WithMessage(ValidationMessages.TemplateImageIsNotImage)
                .When(x => x.Image is not null);

            RuleFor(x => x.TopicId).NotEmpty()
                .WithMessage(ValidationMessages.TemplateTopicId);

            RuleForEach(x => x.Questions).SetValidator(QuestionRequestValidator._questionValidator)
                .When(x => x.Questions is not null && x.Questions.Count != 0);

            RuleFor(x => x.Questions).NotEmpty()
                .WithMessage(ValidationMessages.TemplateQuestionsNotEmpty);

            RuleForEach(x => x.Tags).SetValidator(TagValidator._tagValidator)
                .When(x => x.Tags is not null && x.Tags.Count != 0);
        }
    }
}
