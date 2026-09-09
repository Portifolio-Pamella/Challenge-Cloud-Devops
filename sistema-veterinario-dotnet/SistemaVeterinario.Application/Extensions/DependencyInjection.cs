using Microsoft.Extensions.DependencyInjection;
using SistemaVeterinario.Application.Service;
using SistemaVeterinario.Application.Service.Interface;

namespace SistemaVeterinario.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IVeterinarioService, VeterinarioService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<ITutorService, TutorService>(); // Adicionado para corrigir o Tutor

            return services;
        }
    }
}