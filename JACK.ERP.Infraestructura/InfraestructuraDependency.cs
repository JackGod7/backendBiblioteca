// JACK.ERP.Infraestructura\InfraestructuraDependency.cs
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
            services.AddDbContext<JackContext>(options => options.UseSqlServer(connectionString));

            // Repositorios
            services.AddTransient<IPrestamoRepository, PrestamoRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<ICopiaRepository, CopiaRepository>();

            return services;
        }
    }
}
