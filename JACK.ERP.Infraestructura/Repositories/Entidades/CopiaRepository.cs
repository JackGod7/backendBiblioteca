using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Copia> ObtenerCopiaAsync(int copiaId)
        {
            return await _context.Copias.FindAsync(copiaId);
        }

        // Obtener varias copias en una sola consulta
        public async Task<List<Copia>> ObtenerCopiasPorIdsAsync(IEnumerable<int> copiasIds)
        {
            return await _context.Copias
                                 .Where(c => copiasIds.Contains(c.CopiaId))
                                 .ToListAsync();
        }
    }
}
