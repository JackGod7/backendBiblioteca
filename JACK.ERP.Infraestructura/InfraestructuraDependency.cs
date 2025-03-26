using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using JACK.ERP.Infraestructura.Repositories.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JACK.ERP.Infraestructura
{
    public static class InfraestructuraDependency
    {
        public static IServiceCollection AddInfraestructura(this IServiceCollection services, string? connectionString)
        {
            // Registrar el DbContext
            services.AddDbContext<JackContext>(options => options.UseSqlServer(connectionString));

            // Registrar el repositorio de Prestamos/Alquiler
            services.AddTransient<IPrestamoRepository, PrestamoRepository>();

            // Registras ClienteRepository (para lista negra)
            services.AddTransient<IClienteRepository, ClienteRepository>();
            // Evaluar las copias
            services.AddTransient<ICopiaRepository, CopiaRepository>();

            return services;
        }
    }
}
