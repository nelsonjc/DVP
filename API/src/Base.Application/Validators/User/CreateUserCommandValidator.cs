using FluentValidation;
using TaskingSystem.Application.CQRS.Commands;

namespace TaskingSystem.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            // Validación para FullName
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("El nombre completo es obligatorio.")
                .MaximumLength(250).WithMessage("El nombre completo no puede exceder los 250 caracteres.");

            // Validación para UserName
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder los 50 caracteres.");

            // Validación para Password
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            // Validación para IdRole (Guid)
            RuleFor(x => x.IdRole)
                .NotEmpty().WithMessage("El rol es obligatorio.");
        }
    }
}
