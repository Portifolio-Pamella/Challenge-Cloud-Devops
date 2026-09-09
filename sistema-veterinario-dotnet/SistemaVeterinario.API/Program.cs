using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SistemaVeterinario.Application.Extensions;
using SistemaVeterinario.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Adiciona as camadas
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Adiciona o Liveness check diretamente na API
builder.Services.AddHealthChecks()
    .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(), tags: new[] { "live" });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Mapeamento dos Health Checks exigidos pelo professor
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthorization();
app.MapControllers();

app.Run();