using FluentValidation;
using TaskingSystem.Application.CQRS.Commands;

namespace TaskingSystem.Application.Validators
{
    public class CreateWorkItemCommandValidator : AbstractValidator<CreateWorkItemCommand>
    {
        public CreateWorkItemCommandValidator()
        {
            // Validación para el campo Title (Título)
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(100).WithMessage("El título no debe exceder los 100 caracteres.");

            // Validación para el campo Description (Descripción)
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no debe exceder los 500 caracteres.");

            // Validación para el campo DueDate (Fecha de vencimiento)
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now.AddDays(-1)).WithMessage("La fecha de vencimiento debe ser en el futuro.");

            // Validación para el campo IdUser (Usuario asignado)
            RuleFor(x => x.IdUser)
                .NotEmpty().WithMessage("El usuario asignado es obligatorio.");

            // Validación para el campo IdUserCreated (Usuario que creó el ítem)
            RuleFor(x => x.IdUserCreated)
                .NotEmpty().WithMessage("El usuario creador es obligatorio.");
        }
    }
}
