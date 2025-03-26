using JACK.ERP.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Interfaces
{
    // Si en tu BD y Dominio usas “Alquiler” como entidad,
    // nomenclamos IPrestamoRepository pero apuntando a la entidad Alquiler.
    public interface IPrestamoRepository
    {
        // Retorna todos los alquileres (si lo necesitas)
        Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync();

        // Registra un nuevo alquiler y sus detalles (copias)
        Task RegistrarAlquilerAsync(Alquiler alquiler, List<int> copiasId);

        // Cantidad de copias aún no devueltas por un cliente (para la regla de 3 copias)
        Task<int> ObtenerCopiasActivasDeClienteAsync(int clienteId);
    }
}
