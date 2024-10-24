using FluentValidation;
using FormsCreator.Application.Resources;
using FormsCreator.Core.DTOs.User;

namespace FormsCreator.Application.Validators.User
{
    internal class UserLoginRequestValidator : AbstractValidator<UserLoginRequestDto>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.UserOrEmail).NotEmpty()
                .WithMessage(ValidationMessages.UserLoginUserOrEmail);
        }
    }
}
