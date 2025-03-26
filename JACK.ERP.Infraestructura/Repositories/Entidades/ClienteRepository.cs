// JACK.ERP.Infraestructura\Repositories\Entidades\ClienteRepository.cs
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace JACK.ERP.Infraestructura.Repositories.Entidades
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly JackContext _context;

        public ClienteRepository(JackContext context)
        {
            _context = context;
        }

        public async Task<bool> EstaEnListaNegraAsync(int clienteId)
        {
            return await _context.ListaNegra.AnyAsync(ln => ln.ClienteId == clienteId);
        }
    }
}
