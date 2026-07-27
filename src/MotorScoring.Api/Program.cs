using MotorScoring.Adapters.Inbound.Api.DependencyInjection;
using MotorScoring.Adapters.Inbound.Api.Middleware;
using MotorScoring.Adapters.Outbound.Persistence.DependencyInjection;
using MotorScoring.Api.DependencyInjection;
using MotorScoring.Api.Database;
using MotorScoring.Api.Health;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInboundApi();
builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks().AddCheck<SqlServerHealthCheck>("sqlserver");

var app = builder.Build();

var cs = builder.Configuration.GetConnectionString("MotorScoringDb") ?? throw new InvalidOperationException("Connection string requerida.");

DatabaseMigrationRunner.Run(cs, app.Logger);
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");
app.Run();
public partial class Program
{
}

