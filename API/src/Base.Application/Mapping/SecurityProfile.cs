using AutoMapper;
using TaskingSystem.Application.CQRS.Commands;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Application.Mapping
{
    public class SecurityProfile : Profile
    {
        public SecurityProfile()
        {
            CreateMap<CreateUserCommand, User>(); ;
            CreateMap<User, UserDto>()
                .ForMember(x => x.UserCreated, o => o.MapFrom(s => s.UserCreated.FullName))
                .ForMember(x => x.UserUpdated, o => o.MapFrom(s => s.UserUpdated != null ? s.UserUpdated.FullName : null))
                .ForMember(x => x.Role, o => o.MapFrom(s => s.Role.Name))
                .ForMember(x => x.TaskInProcess, o => o.MapFrom(s => s.Tasks.Where(p => p.Status.Code == "IN_PROCESS").Count()))
                .ForMember(x => x.TaskPending, o => o.MapFrom(s => s.Tasks.Where(p => p.Status.Code == "PENDING").Count()))
                .ForMember(x => x.TaskCompleted, o => o.MapFrom(s => s.Tasks.Where(p => p.Status.Code == "COMPLETED").Count()))
                .ForMember(x => x.TaskDue, o => o.MapFrom(s => s.Tasks.Where(p => p.Status.Code != "COMPLETED" && DateTime.UtcNow.AddHours(-5).Date > p.DueDate.Date).Count()))
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(
                    src => src.Role.RolPermissions.Select(
                        rp => new PermissionDto
                        {
                            Id = rp.Permission.Id,
                            Entity = rp.Permission.Entity,
                            IdActionType = (int)rp.Permission.ActionType,
                            ActionType = rp.Permission.ActionType.ToString(),
                            DateCreated = rp.Permission.DateCreated,
                            IdUserCreated = rp.Permission.IdUserCreated,
                            UserCreated = rp.Permission.UserCreated != null ? rp.Permission.UserCreated.FullName : null,
                            DateUpdated = rp.Permission.DateUpdated != null ? rp.Permission.DateUpdated : null,
                            IdUserUpdated = rp.Permission.IdUserUpdated != null ? rp.Permission.IdUserUpdated : null,
                            UserUpdated = rp.Permission.UserUpdated != null ? rp.Permission.UserUpdated.FullName : null,
                        })
                    )
                );

            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.RolPermissions, opt => opt.MapFrom(
                    src => src.RolPermissions.Select(
                        rp => new PermissionDto
                        {
                            Id = rp.Permission.Id,
                            Entity = rp.Permission.Entity,
                            IdActionType = (int)rp.Permission.ActionType,
                            ActionType = rp.Permission.ActionType.ToString(),
                            DateCreated = rp.Permission.DateCreated,
                            IdUserCreated = rp.Permission.IdUserCreated,
                            UserCreated = rp.Permission.UserCreated.FullName,
                            DateUpdated = rp.Permission.DateUpdated != null ? rp.Permission.DateUpdated : null,
                            IdUserUpdated = rp.Permission.IdUserUpdated != null ? rp.Permission.IdUserUpdated : null,
                            UserUpdated = rp.Permission.UserUpdated != null ? rp.Permission.UserUpdated.FullName : null,
                        })
                    )
                );
        }
    }
}
