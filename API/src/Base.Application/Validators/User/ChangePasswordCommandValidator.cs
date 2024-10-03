using FluentValidation;
using TaskingSystem.Application.CQRS.Commands;

namespace TaskingSystem.Application.Validators
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            // Validar IdUser no debe ser un Guid vacío
            RuleFor(command => command.IdUser)
                .NotEqual(Guid.Empty).WithMessage("El Usuario a modificar no puede enviarse vacío");

            // Validar IdUserUpdated no debe ser un Guid vacío
            RuleFor(command => command.IdUserUpdated)
                .NotEqual(Guid.Empty).WithMessage("El Usuario modificador no puede enviarse vacío.");

            // Validar Password no debe ser nulo o vacío y debe tener un mínimo de 6 caracteres
            RuleFor(command => command.Password)
                .NotEmpty().WithMessage("La contraseña es requerida.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
