using AutoMapper;
using TaskingSystem.Application.Constants;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Exceptions;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class ChangeStatusWorkItemCommandHandler(
        IMapper mapper, 
        IStatusRepository statusRepository, 
        IUserRepository userRepository,
        IWorkItemRepository repository) : ICommandHandler<ChangeStatusWorkItemCommand, ApiResponse<bool>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IStatusRepository _statusRepository = statusRepository;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IWorkItemRepository _repository = repository;

        public async Task<ApiResponse<bool>> Handle(ChangeStatusWorkItemCommand command, CancellationToken cancellation)
        {
            try
            {
                DateTime now = DateTime.UtcNow.AddHours(-5);
                var user = await _userRepository.GetByIdAsync(command.IdUserUpdated) ?? throw new NotFoundException(nameof(User), command.IdUserUpdated);
                var workItem = await _repository.GetByIdAsync(command.Id) ?? throw new NotFoundException(nameof(WorkItem), command.Id);

                var statusCurrency = workItem.Status;

                var newStatus = await _statusRepository.GetByIdAsync(command.IdStatus);

                workItem.IdStatus = command.IdStatus;
                workItem.IdUserUpdated = user.Id;
                workItem.DateUpdated = now;

                // Guarda los cambios de la actualización
                await _repository.UpdateAsync(workItem);
                await _repository.SaveChangeAsync();

                string changes = $"<strong>Estado</strong>: Ha pasado de '{statusCurrency.Name}' a '{newStatus.Name}' <br/>" +
                    $"<strong>Observación</strong>: {command.Observation} <br/>" +
                    $"<strong>Usuario</strong>: {user.FullName}";

                // Log de eventos
                workItem.Logs.Add(new WorkItemLog
                {
                    IdWorkItem = workItem.Id,
                    IdUserCreated = command.IdUserUpdated,
                    TypeEvent = LogEnventConstant.CHANGE_STATUS,
                    LogEvent = changes
                });

                // Flujo de estados                    
                workItem.Flows.Add(new WorkItemFlow
                {
                    IdWorkItem = workItem.Id, // Asegúrate de asignar el IdWorkItem aquí
                    IdUserCreated = command.IdUserUpdated,
                    IdNewStatus = command.IdStatus,
                    IdPreviousStatus = statusCurrency.Id,
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
