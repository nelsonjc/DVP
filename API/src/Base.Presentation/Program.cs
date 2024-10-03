using Microsoft.EntityFrameworkCore;
using TaskingSystem.Infrastructure.Persistencia;
using TaskingSystem.Presentation.App_Start;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using TaskingSystem.Presentation.Middleware;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Aquí puedes agregar tus validadores y configuraciones de servicios
builder.RegisterValidator();


builder.Services.AddControllers();

// DI Configuration
builder.RegisterDI();
builder.ConfigureMap();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsPolicy("AllowSpecificOrigin", ["http://localhost:4200"]);

var conn = SwitchConnectionConfiguration.GetConnection();
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(conn, p => p.MigrationsAssembly("TaskingSystem.Presentation")));

// Configuración de Serilog
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    .MinimumLevel.Override("System", LogEventLevel.Warning)
//    .WriteTo.Console()
//    .WriteTo.File("logs/app.txt", rollingInterval: RollingInterval.Day)
//    .WriteTo.MSSqlServer(conn, new MSSqlServerSinkOptions
//    {
//        TableName = "Logs",
//        SchemaName = "dbo",
//        AutoCreateSqlTable = true
//    })
//    .WriteTo.Seq("http://localhost:5341")
//    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Aquí se agrega el middleware de registro
builder.Services.AddScoped<LoggingMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Tasking System API", Version = "v1" });
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Invoca el middleware de registro después de los filtros de validación
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<LoggingMiddleware>(); // Asegúrate de que esto esté después de la autorización
app.MapControllers();
app.UseCors("AllowSpecificOrigin");
app.Run();

Log.CloseAndFlush();

