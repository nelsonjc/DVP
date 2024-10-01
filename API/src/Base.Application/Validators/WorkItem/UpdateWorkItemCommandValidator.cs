using FluentValidation;
using TaskingSystem.Application.CQRS.Commands;

namespace TaskingSystem.Application.Validators
{
    public class UpdateWorkItemCommandValidator : AbstractValidator<UpdateWorkItemCommand>
    {
        public UpdateWorkItemCommandValidator()
        {
            // Validación para el campo Id (Identificador del ítem)
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El identificador del ítem es obligatorio.");

            // Validación para el campo Title (Título)
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .MaximumLength(100).WithMessage("El título no debe exceder los 100 caracteres.");

            // Validación para el campo Description (Descripción)
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no debe exceder los 1000 caracteres.");

            // Validación para el campo DueDate (Fecha de vencimiento)
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now).WithMessage("La fecha de vencimiento debe ser en el futuro.");

            // Validación para el campo IdUser (Usuario asignado)
            RuleFor(x => x.IdUser)
                .NotEmpty().WithMessage("El usuario asignado es obligatorio.");

            // Validación para el campo IdStatus (Estado)
            RuleFor(x => x.IdStatus)
                .NotEmpty().WithMessage("El estado es obligatorio.");

            // Validación para el campo IdUserUpdated (Usuario que actualizó el ítem)
            RuleFor(x => x.IdUserUpdated)
                .NotEmpty().WithMessage("El usuario que actualizó el ítem es obligatorio.");
        }
    }
}
