using AutoMapper;
using TaskingSystem.Application.Constants;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class UpdateWorkItemCommandHandler(IPropertyComparer<WorkItemDto> propertyComparer, IMapper mapper, IWorkItemRepository repository) : ICommandHandler<UpdateWorkItemCommand, ApiResponse<bool>>
    {
        private readonly IPropertyComparer<WorkItemDto> _propertyComparer = propertyComparer;
        private readonly IMapper _mapper = mapper;
        private readonly IWorkItemRepository _repository = repository;

        public async Task<ApiResponse<bool>> Handle(UpdateWorkItemCommand command, CancellationToken cancellationToken)
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(-5);
                var workItem = await _repository.GetByIdSimple(command.Id) ?? throw new NotFoundException(nameof(User), command.Id);
                var previousStatus = workItem.IdStatus;
                var workItemOriginal = _mapper.Map<WorkItemDto>(workItem);

                // Actualiza las propiedades del workItem
                workItem.Title = command.Title;
                workItem.Description = command.Description;
                workItem.DueDate = command.DueDate;
                workItem.IdStatus = command.IdStatus;
                workItem.IdUserUpdated = command.IdUserUpdated;
                workItem.IdUser = command.IdUser;
                workItem.DateUpdated = now;

                // Guarda los cambios de la actualización
                await _repository.UpdateAsync(workItem);
                await _repository.SaveChangeAsync();

                // Comparación de propiedades y creación del log
                var workItemCopy = _mapper.Map<WorkItemDto>(workItem); // Crea una copia actualizada
                workItemCopy.DateUpdated = now;
                string changes = _propertyComparer.Compare(workItemOriginal, workItemCopy);
                changes = $"{changes}<strong>Observación</strong>: {command.Observation}";

                // Log de eventos
                workItem.Logs.Add(new WorkItemLog
                {
                    IdWorkItem = workItem.Id,
                    IdUserCreated = command.IdUserUpdated,
                    TypeEvent = LogEnventConstant.UPDATE,
                    LogEvent = changes
                });

                // Flujo de estados                    
                workItem.Flows.Add(new WorkItemFlow
                {
                    IdWorkItem = workItem.Id, // Asegúrate de asignar el IdWorkItem aquí
                    IdUserCreated = command.IdUserUpdated,
                    IdNewStatus = command.IdStatus,
                    IdPreviousStatus = previousStatus,
                    Observation = command.Observation
                });

                // Guarda los cambios
                await _repository.SaveChangeAsync();
                return ApiResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }


    }
}
