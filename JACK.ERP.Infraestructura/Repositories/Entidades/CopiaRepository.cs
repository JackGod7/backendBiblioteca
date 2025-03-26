// JACK.ERP.Infraestructura\Repositories\Entidades\CopiaRepository.cs
using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JACK.ERP.Infraestructura.Repositories.Entidades
{
    /// <summary>
    /// CopiaRepository maneja las operaciones de acceso a datos
    /// para la entidad Copia, incluyendo la obtención de múltiples copias
    /// según una lista de IDs.
    /// </summary>
    public class CopiaRepository : ICopiaRepository
    {
        private readonly JackContext _context;


        /// <summary>
        /// Constructor que recibe el contexto de EF para interactuar con la base de datos.
        /// </summary>
        public CopiaRepository(JackContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de Copias filtradas por un conjunto de IDs. 
        /// Esta operación se usa para verificar la disponibilidad de copias
        /// o para cargar información de cada una antes de un préstamo.
        /// </summary>
        public async Task<List<Copia>> ObtenerCopiasPorIdsAsync(IEnumerable<int> copiaIds)
        {
            return await _context.Copias
                .Where(c => copiaIds.Contains(c.CopiaId))
                .ToListAsync();
        }
    }
}
