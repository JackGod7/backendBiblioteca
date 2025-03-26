using JACK.ERP.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Interfaces
{
    /// <summary>
    /// Define las operaciones de acceso a datos (CRUD o lecturas especializadas)
    /// relacionadas con los préstamos (Alquiler) y sus detalles.
    /// </summary>
    public interface IPrestamoRepository
    {
        /// <summary>
        /// Obtiene la lista de todos los alquileres registrados, incluyendo
        /// información de cliente y detalle de copias asociadas.
        /// </summary>
        /// <returns>Colección de objetos Alquiler</returns>
        Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync();

        /// <summary>
        /// Registra un nuevo alquiler en la base de datos, creando
        /// también los AlquilerDetalle por cada copia solicitada
        /// y cambiando el estado de las copias a “Prestado”.
        /// </summary>
        /// <param name="alquiler">Entidad Alquiler a guardar</param>
        /// <param name="copiasId">Lista de IDs de copias que se asocian al Alquiler</param>
        Task RegistrarAlquilerAsync(Alquiler alquiler, List<int> copiasId);

        /// <summary>
        /// Indica cuántas copias tiene un cliente en préstamo activo 
        /// (alquileres sin FechaDevolucion).
        /// Se usa para verificar la regla de “máximo 3 copias”.
        /// </summary>
        /// <param name="clienteId">ID del cliente a consultar</param>
        /// <returns>Cantidad de copias activas</returns>
        Task<int> ObtenerCopiasActivasDeClienteAsync(int clienteId);
    }
}
