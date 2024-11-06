using FluentValidation;
using FormsCreator.Application.Records;

namespace FormsCreator.Application.Validators.Records
{
    internal class JiraIssueValidator : AbstractValidator<JiraIssue>
    {
        public JiraIssueValidator()
        {
            RuleFor(x => x.Priority).IsInEnum()
                .WithMessage("Not valid enum value.");
        }
    }
}
