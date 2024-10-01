using FluentValidation;
using Newtonsoft.Json;
using TaskingSystem.Application.Responses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace TaskingSystem.Presentation.Middleware
{
    /// <summary>
    /// Middleware para registrar información de los controladores y acciones que están siendo ejecutados en la aplicación.
    /// </summary>
    public class LoggingMiddleware : IMiddleware
    {
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Constructor que inicializa el logger para el middleware.
        /// </summary>
        /// <param name="logger">Instancia de <see cref="ILogger{TCategoryName}"/> para registrar información.</param>
        public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Método que se ejecuta con cada solicitud HTTP. Registra el nombre del controlador y acción, y maneja cualquier excepción.
        /// </summary>
        /// <param name="context">El <see cref="HttpContext"/> que representa la solicitud actual.</param>
        /// <param name="next">El delegado <see cref="RequestDelegate"/> que representa el siguiente middleware en la cadena.</param>
        /// <returns>Una tarea asíncrona.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Obtiene el nombre del controlador y la acción desde la solicitud actual
                var controllerName = context.Request.RouteValues["controller"];
                var actionName = context.Request.RouteValues["action"];

                // Registra la información del controlador y acción
                _logger.LogInformation("Logging from {Controller}/{Action}.", controllerName, actionName);

                // Invoca el siguiente middleware en la cadena
                await next(context);
            }
            catch (Exception ex)
            {
                // Registra cualquier excepción que ocurra durante la ejecución de la solicitud
                _logger.LogError(ex, "Exception occurred while processing the request.");
                throw;  // Propaga la excepción
            }
        }
    }
}
