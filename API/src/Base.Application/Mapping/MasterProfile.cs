using AutoMapper;
using TaskingSystem.Application.DTOs;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Application.Mapping
{
    public class MasterProfile : Profile
    {
        public MasterProfile()
        {
            CreateMap<StatusSystem, StatusDto>();
        }
    }
}
