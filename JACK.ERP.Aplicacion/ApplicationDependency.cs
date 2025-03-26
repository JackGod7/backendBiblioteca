using JACK.ERP.Aplicacion.Interfaces.Entidades;
using JACK.ERP.Aplicacion.Services.Entidades;
using Microsoft.Extensions.DependencyInjection;

namespace JACK.ERP.Aplicacion
{
    /// <summary>
    /// Clase de extensión para inyectar los servicios de la capa de Aplicación
    /// (por ejemplo, PrestamoService) en el contenedor de dependencias de .NET.
    /// </summary>
    public static class ApplicationDependency
    {
        /// <summary>
        /// Registra los servicios de la capa de aplicación (como IPrestamoService)
        /// a sus implementaciones (PrestamoService).
        /// </summary>
        /// <param name="services">La colección de servicios .NET</param>
        /// <returns>La misma colección, para encadenar llamadas</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Asocia IPrestamoService con PrestamoService
            services.AddTransient<IPrestamoService, PrestamoService>();
            return services;
        }
    }
}
