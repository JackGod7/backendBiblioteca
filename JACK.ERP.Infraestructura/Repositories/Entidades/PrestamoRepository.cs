// JACK.ERP.Infraestructura\Repositories\Entidades\PrestamoRepository.cs
using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using JACK.ERP.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;


namespace JACK.ERP.Infraestructura.Repositories.Entidades
{
    /// <summary>
    /// PrestamoRepository implementa la lógica de acceso a datos 
    /// asociada a los préstamos: Alquiler y AlquilerDetalle.
    /// Se encarga de guardar y leer información del contexto EF.
    /// </summary>
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly JackContext _context;

        /// <summary>
        /// Inyecta el DbContext para manipular tablas Alquiler, AlquilerDetalle y Copia.
        /// </summary>
        public PrestamoRepository(JackContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene todos los préstamos (Alquileres), incluyendo
        /// la información del Cliente y la lista de Detalles con sus Copias.
        /// </summary>
        public async Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync()
        {
            // Cargar Detalles y Copias
            return await _context.Alquileres
                .Include(a => a.Cliente)
                .Include(a => a.Detalles).ThenInclude(d => d.Copia)
                .ToListAsync();
        }


        /// <summary>
        /// Registra un nuevo Alquiler y sus Detalles, asignando el estado "Prestado" 
        /// a las copias. Usa una transacción para asegurar que se guarden 
        /// de forma atómica (todo o nada).
        /// </summary>
        /// <param name="alquiler">Entidad Alquiler que se va a guardar en la tabla</param>
        /// <param name="copiasId">Lista de IDs de copias a asociar al Alquiler</param>
        public async Task RegistrarAlquilerAsync(Alquiler alquiler, List<int> copiasId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                
                await _context.Alquileres.AddAsync(alquiler);
                await _context.SaveChangesAsync();

                
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
                        throw new Exception($"No se encontró la copia con ID={cid}"); 
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


        /// <summary>
        /// Cuenta cuántas copias aún no devueltas tiene un cliente (Alquileres sin fecha de devolución).
        /// Se suman los detalles de cada alquiler activo para conocer cuántas copias mantiene prestadas.
        /// </summary>
        /// <param name="clienteId">Id del cliente a consultar</param>
        /// <returns>Número total de copias activas</returns>
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
