using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskingSystem.Application.CQRS.Commands;

namespace TaskingSystem.Application.Validators
{
    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(command => command.IdUser)

                 .NotEqual(Guid.Empty).WithMessage("El  Id del Usuario a desactivar no puede enviarse vacío");

            RuleFor(command => command.IdUserUpdated)

                .NotEqual(Guid.Empty).WithMessage("El Id del usuario que activa no puede enviarse vacío");
        }
    }
}
