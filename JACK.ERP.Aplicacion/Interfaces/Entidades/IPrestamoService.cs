using JACK.ERP.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JACK.ERP.Aplicacion.Interfaces.Entidades
{
    public interface IPrestamoService
    {
        // Obtener todos los Alquileres.
        Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync();

        Task<Alquiler> RegistrarPrestamoAsync(int clienteId, List<int> copiasId, DateTime fechaFin);
    }
}
