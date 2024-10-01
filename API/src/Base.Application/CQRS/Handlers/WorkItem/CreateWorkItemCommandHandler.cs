using AutoMapper;
using TaskingSystem.Application.Constants;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Responses;
using TaskingSystem.Domain.Entities;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;

namespace TaskingSystem.Application.CQRS.Handlers
{
    public class CreateWorkItemCommandHandler(
        IPropertyComparer<WorkItem> propertyComparer,
        IWorkItemRepository repository, 
        IStatusRepository statusRepository, 
        IMapper mapper) : ICommandHandler<CreateWorkItemCommand, ApiResponse<Guid>>
    {
        private readonly IPropertyComparer<WorkItem> _propertyComparer = propertyComparer;
        private readonly IWorkItemRepository _repository = repository;
        private readonly IStatusRepository _statusRepository = statusRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse<Guid>> Handle(CreateWorkItemCommand command, CancellationToken cancellation)
        {
            try
            {
                var workItemDb = _mapper.Map<WorkItem>(command);

                //Obtengo el id
                var initialStatus = (await _statusRepository.FindAsync(x => x.Code == "PENDING")).FirstOrDefault();

                if (initialStatus != null)
                {
                    workItemDb.IdStatus = initialStatus.Id;
                    workItemDb.Active = true;

                    // Flujo de estados                    
                    workItemDb.Flows.Add(new WorkItemFlow() {
                        IdWorkItem = workItemDb.Id,
                        IdUserCreated = command.IdUserCreated, 
                        IdNewStatus = initialStatus.Id, 
                        IdPreviousStatus = initialStatus.Id, 
                        Observation = MessageConstant.TASK_CREATION_OBSERVATION 
                    });

                    // Log de eventos
                    string changes = _propertyComparer.Compare(null, workItemDb);
                    workItemDb.Logs.Add(new WorkItemLog { 
                        IdWorkItem = workItemDb.Id,
                        IdUserCreated = command.IdUserCreated, 
                        TypeEvent = LogEnventConstant.CREATE, 
                        LogEvent = changes 
                    });                   

                    await _repository.AddAsync(workItemDb);
                    return ApiResponse<Guid>.SuccessResponse(workItemDb.Id);
                }
                else
                    throw new Exception("El estado 'Pendiente' no se encuentra registrado en la base de datos.");
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.ErrorResponse(ex.Message);
            }
        }
    }
}
