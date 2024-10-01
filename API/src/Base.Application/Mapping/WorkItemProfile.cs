using AutoMapper;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Application.Mapping
{
    public class WorkItemProfile : Profile
    {
        public WorkItemProfile()
        {
            CreateMap<CreateWorkItemCommand, WorkItem>();            
            CreateMap<UpdateWorkItemCommand, WorkItemDto>();            
            CreateMap<WorkItem, WorkItemDto>()
                .ForMember(x => x.Status, o => o.MapFrom(s => s.Status.Name))
                .ForMember(x => x.StatusCode, o => o.MapFrom(s => s.Status.Code))
                .ForMember(x => x.User, o => o.MapFrom(s => s.User.FullName))
                .ForMember(x => x.UserCreated, o => o.MapFrom(s => s.UserCreated.FullName))
                .ForMember(x => x.UserUpdated, o => o.MapFrom(s => s.UserUpdated != null ? s.UserUpdated.FullName : null));

            CreateMap<WorkItemFlow, WorkItemFlowDto>()
                .ForMember(x => x.PreviousStatus, o => o.MapFrom(s => s.PreviousStatus.Name))
                .ForMember(x => x.NewStatus, o => o.MapFrom(s => s.NewStatus.Name))
                .ForMember(x => x.UserCreated, o => o.MapFrom(s => s.UserCreated.FullName));

            CreateMap<WorkItemLog, WorkItemLogDto>().ForMember(x => x.UserCreated, o => o.MapFrom(s => s.UserCreated.FullName));

        }
    }
}
