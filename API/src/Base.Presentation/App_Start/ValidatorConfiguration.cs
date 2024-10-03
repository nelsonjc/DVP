using FluentValidation.AspNetCore;
using FluentValidation;
using TaskingSystem.Application.Validators;

namespace TaskingSystem.Presentation.App_Start
{
    public static class ValidatorConfiguration
    {
        public static void RegisterValidator(this WebApplicationBuilder builder)
        {
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateWorkItemCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ChangePasswordCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<DeleteUserCommandValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<ActivateUserCommandValidator>();
        }
    }
}
