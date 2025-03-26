// JACK.ERP.Infraestructura\Repositories\Entidades\PrestamoRepository.cs
using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;


namespace JACK.ERP.Infraestructura.Repositories.Entidades
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly JackContext _context;

        public PrestamoRepository(JackContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync()
        {
            // Cargar Detalles y Copias
            return await _context.Alquileres
                .Include(a => a.Cliente)
                .Include(a => a.Detalles).ThenInclude(d => d.Copia)
                .ToListAsync();
        }

        public async Task RegistrarAlquilerAsync(Alquiler alquiler, List<int> copiasId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Insertar cabecera
                await _context.Alquileres.AddAsync(alquiler);
                await _context.SaveChangesAsync();

                // Insertar detalle + actualizar estado de copias
                foreach (var cid in copiasId)
                {
                    var detalle = new AlquilerDetalle
                    {
                        AlquilerId = alquiler.AlquilerId,
                        CopiaId = cid
                    };
                    await _context.AlquilerDetalles.AddAsync(detalle);

                    var copia = await _context.Copias.FindAsync(cid);
                    if (copia == null)
                        throw new Exception($"No se encontró la copia con ID={cid}"); // debería no ocurrir, revisado antes
                    copia.Estado = CopiaEstado.Prestado;
                    _context.Copias.Update(copia);
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
            // Alquileres sin fechaDevolucion => no devuelto
            var alquileresActivos = await _context.Alquileres
                .Where(a => a.ClienteId == clienteId && a.FechaDevolucion == null)
                .Include(a => a.Detalles)
                .ToListAsync();

            return alquileresActivos.Sum(a => a.Detalles.Count);
        }
    }
}
