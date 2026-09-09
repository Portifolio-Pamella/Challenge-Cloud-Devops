using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SistemaVeterinario.Infrastructure.Data;
using SistemaVeterinario.Infrastructure.Repositories;

namespace SistemaVeterinario.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro do DbContext do Entity Framework com Oracle usando o nome correto da connection string
        services.AddDbContext<AppDbContext>(options =>
            options.UseOracle(configuration.GetConnectionString("OracleConnection")));

        // Registros dos repositórios
        services.AddScoped<SistemaVeterinario.Infrastructure.Repositories.Interfaces.IPetRepository, PetRepository>();
        services.AddScoped<SistemaVeterinario.Infrastructure.Repositories.Interfaces.ITutorRepository, TutorRepository>();
        services.AddScoped<SistemaVeterinario.Infrastructure.Repositories.Interfaces.IVeterinarioRepository, VeterinarioRepository>();

        // Configuração do OpenTelemetry
        services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddConsoleExporter());

        return services;
    }
}