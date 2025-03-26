using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Interfaces
{
    /// <summary>
    /// Define las operaciones de acceso a datos relacionadas con la entidad Cliente,
    /// incluidas validaciones de lista negra.
    /// </summary>
    public interface IClienteRepository
    {
        /// <summary>
        /// Verifica si un cliente se encuentra en la lista negra, consultando la tabla ListaNegra.
        /// Retorna true si existe un registro de dicho cliente en la lista, de lo contrario false.
        /// </summary>
        /// <param name="clienteId">ID del cliente a consultar</param>
        /// <returns>Indica si el cliente está bloqueado en la lista negra</returns>
        Task<bool> EstaEnListaNegraAsync(int clienteId);
    }
}
