using AutoMapper.Execution;
using TaskingSystem.Application.CQRS.Common;
using TaskingSystem.Application.Services;
using TaskingSystem.Domain.Interfaces.Repository;
using TaskingSystem.Domain.Interfaces.Services;
using TaskingSystem.Domain.Options;
using TaskingSystem.Infrastructure.Repositories;
using TaskingSystem.Presentation.Dispatcher;

namespace TaskingSystem.Presentation.App_Start
{
    /// <summary>
    /// Static class for configuring Dependency Injection (DI) registrations in the application.
    /// </summary>
    public static class DIConfiguration
    {
        public static void RegisterDI(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IStatusRepository, StatusRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();

            builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            builder.Services.AddTransient<IPasswordService, PasswordService>();
            builder.Services.AddTransient<IReplaceVisitorService, ReplaceVisitorService>();
            builder.Services.AddTransient<ITreeModifierService, TreeModifierService>();
            builder.Services.AddTransient(typeof(IPropertyComparer<>), typeof(PropertyComparer<>));

            builder.RegisterRepository();
            builder.RegisterQueryHandlers();
            builder.RegisterCommands();
            builder.RegisterOptions();
        }

        /// <summary>
        /// Registra automáticamente todas las implementaciones de la interfaz genérica IRepository en el contenedor de inyección de dependencias.
        /// Escanea los ensamblados cargados en el dominio de la aplicación y busca clases que implementen IRepository.
        /// Agrega esas clases al servicio de inyección de dependencias como Scoped.
        /// </summary>
        /// <param name="builder">Instancia del WebApplicationBuilder utilizada para configurar los servicios.</param>
        private static void RegisterRepository(this WebApplicationBuilder builder)
        {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>))))
            {
                foreach (var interfaceType in type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
                {
                    // Registra la clase concreta como un servicio Scoped para la interfaz IRepository<>.
                    builder.Services.AddScoped(interfaceType, type);
                }
            }
        }

        /// <summary>
        /// Registra automáticamente todas las implementaciones de la interfaz genérica IQueryHandler en el contenedor de inyección de dependencias.
        /// Escanea los ensamblados cargados y busca clases que implementen IQueryHandler.
        /// Agrega esas clases al servicio de inyección de dependencias como Scoped.
        /// </summary>
        /// <param name="builder">Instancia del WebApplicationBuilder utilizada para configurar los servicios.</param>
        private static void RegisterQueryHandlers(this WebApplicationBuilder builder)
        {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
            {
                foreach (var interfaceType in type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                {
                    // Registra la clase concreta como un servicio Scoped para la interfaz IQueryHandler<,>.
                    builder.Services.AddScoped(interfaceType, type);
                }
            }
        }

        /// <summary>
        /// Registra automáticamente todas las implementaciones de la interfaz genérica ICommandHandler en el contenedor de inyección de dependencias.
        /// Escanea los ensamblados cargados y busca clases que implementen ICommandHandler.
        /// Agrega esas clases al servicio de inyección de dependencias como Scoped.
        /// </summary>
        /// <param name="builder">Instancia del WebApplicationBuilder utilizada para configurar los servicios.</param>
        private static void RegisterCommands(this WebApplicationBuilder builder)
        {
            foreach (var type in AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>))))
            {
                foreach (var interfaceType in type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
                {
                    // Registra la clase concreta como un servicio Scoped para la interfaz ICommandHandler<,>.
                    builder.Services.AddScoped(interfaceType, type);
                }
            }
        }

        private static void RegisterOptions(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.Services.Configure<PaginationOptions>(configuration.GetSection("PaginationOptions"));
            builder.Services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));
        }
    }
}
