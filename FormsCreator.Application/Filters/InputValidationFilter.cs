using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FormsCreator.Application.Filters
{
    public sealed class InputValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.ModelState.Clear();

            var actionArguments = context.ActionArguments.Values;

            foreach (var argument in actionArguments)
            {
                if (argument is null) continue;

                Type argType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                if (serviceProvider.GetService(argType) is IValidator validator)
                {
                    var validationResult = await validator.ValidateAsync(new ValidationContext<object>(argument));
                    if (!validationResult.IsValid)
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                        }
                    }
                }
            }

            await next();
        }
    }
}
