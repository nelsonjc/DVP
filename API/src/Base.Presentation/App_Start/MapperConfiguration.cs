using TaskingSystem.Application.Mapping;

namespace TaskingSystem.Presentation.App_Start
{
    public static class MapperConfiguration
    {
        public static void ConfigureMap(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(SecurityProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(WorkItemProfile).Assembly);
            builder.Services.AddAutoMapper(typeof(MasterProfile).Assembly);
        }
    }
}
