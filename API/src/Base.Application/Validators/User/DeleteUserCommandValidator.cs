using FluentValidation;

using TaskingSystem.Application.CQRS.Commands;

namespace TaskingSystem.Application.Validators
{
    public  class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {

        public DeleteUserCommandValidator()
        {
            RuleFor(command => command.IdUser)
                
                .NotEqual(Guid.Empty).WithMessage("El  Id del Usuario a desactivar no puede enviarse vacío");

            RuleFor(command => command.IdUserUpdated)
               
                .NotEqual(Guid.Empty).WithMessage("El Id del usuario que activa no puede enviarse vacío");
        }
    }
}
