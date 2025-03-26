using JACK.ERP.Aplicacion.Interfaces.Entidades;
using JACK.ERP.Aplicacion.Services.Entidades;
using Microsoft.Extensions.DependencyInjection;

namespace JACK.ERP.Aplicacion
{
    public static class ApplicationDependency
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registramos el servicio de préstamos (IPrestamoService -> PrestamoService)
            services.AddTransient<IPrestamoService, PrestamoService>();
            return services;
        }
    }
}
