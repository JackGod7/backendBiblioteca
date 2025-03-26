// JACK.ERP.Infraestructura\Repositories\Entidades\ClienteRepository.cs
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JACK.ERP.Infraestructura.Repositories.Entidades
{
    /// <summary>
    /// ClienteRepository implementa las operaciones de acceso a datos
    /// relacionadas con la entidad Cliente y su relación con ListaNegra.
    /// </summary>
    public class ClienteRepository : IClienteRepository
    {
        private readonly JackContext _context;

        /// <summary>
        /// Inyecta el DbContext para realizar consultas a la base de datos.
        /// </summary>
        public ClienteRepository(JackContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Indica si el cliente está en la lista negra, consultando la tabla ListaNegra.
        /// Retorna True si existe un registro para el clienteId, caso contrario False.
        /// </summary>
        public async Task<bool> EstaEnListaNegraAsync(int clienteId)
        {
            return await _context.ListaNegra.AnyAsync(ln => ln.ClienteId == clienteId);
        }
    }
}
