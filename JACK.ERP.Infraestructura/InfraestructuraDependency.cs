// JACK.ERP.Infraestructura\InfraestructuraDependency.cs
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using JACK.ERP.Infraestructura.Repositories.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JACK.ERP.Infraestructura
{
    /// <summary>
    /// InfraestructuraDependency registra la capa de Infraestructura.
    /// - Configura el DbContext (JackContext) con la cadena de conexión
    /// - Inyecta las implementaciones de repositorios (Prestamo, Cliente, Copia)
    /// </summary>

    public static class InfraestructuraDependency
    {

        /// <summary>
        /// Método de extensión que agrega la configuración de la capa de infraestructura
        /// a la colección de servicios. Utiliza la cadena de conexión para EF Core.
        /// </summary>
        /// <param name="services">Colección de servicios .NET Core</param>
        /// <param name="connectionString">Cadena de conexión a la base de datos</param>
        /// <returns>La colección de servicios actualizada</returns>
        public static IServiceCollection AddInfraestructura(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<JackContext>(options => options.UseSqlServer(connectionString));

            // Repositorios
            services.AddTransient<IPrestamoRepository, PrestamoRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<ICopiaRepository, CopiaRepository>();

            return services;
        }
    }
}
