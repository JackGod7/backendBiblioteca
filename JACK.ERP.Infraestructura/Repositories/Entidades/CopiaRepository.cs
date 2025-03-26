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
    public class CopiaRepository : ICopiaRepository
    {
        private readonly JackContext _context;

        public CopiaRepository(JackContext context)
        {
            _context = context;
        }

        public async Task<List<Copia>> ObtenerCopiasPorIdsAsync(IEnumerable<int> copiaIds)
        {
            return await _context.Copias
                .Where(c => copiaIds.Contains(c.CopiaId))
                .ToListAsync();
        }
    }
}
