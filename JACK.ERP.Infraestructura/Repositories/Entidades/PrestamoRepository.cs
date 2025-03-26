using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JACK.ERP.Infraestructura.Repositories.Entidades
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly JackContext _context;

        public PrestamoRepository(JackContext context)
        {
            _context = context;
        }

        // En PrestamoRepository (ObtenerPrestamosAsync):
        public async Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync()
        {
            // Incluimos también el cliente
            return await _context.Alquileres
                .Include(a => a.Cliente)
                .Include(a => a.Detalles)
                .ThenInclude(d => d.Copia)
                .ToListAsync();
        }


        public async Task RegistrarAlquilerAsync(Alquiler alquiler, List<int> copiasId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Alquileres.AddAsync(alquiler);
                await _context.SaveChangesAsync();

                foreach (var copiaId in copiasId)
                {
                    var detalle = new AlquilerDetalle
                    {
                        AlquilerId = alquiler.AlquilerId,
                        CopiaId = copiaId
                    };
                    await _context.AlquilerDetalles.AddAsync(detalle);

                    var copia = await _context.Copias.FindAsync(copiaId);
                    if (copia != null)
                    {
                        copia.Estado = "Prestado";
                        _context.Copias.Update(copia);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> ObtenerCopiasActivasDeClienteAsync(int clienteId)
        {
            var alquileresActivos = await _context.Alquileres
                .Where(a => a.ClienteId == clienteId && a.FechaDevolucion == null)
                .Include(a => a.Detalles)
                .ToListAsync();

            int totalCopias = alquileresActivos.Sum(a => a.Detalles.Count);
            return totalCopias;
        }
    }
}
