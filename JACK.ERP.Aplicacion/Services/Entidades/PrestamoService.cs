using JACK.ERP.Aplicacion.Interfaces.Entidades;
using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JACK.ERP.Aplicacion.Services.Entidades
{
    /// <summary>
    /// PrestamoService contiene la lógica de negocio para la gestión de préstamos (Alquiler).
    /// Orquesta los repositorios, valida reglas (lista negra, máximo de copias, disponibilidad)
    /// y registra logs de cada acción.
    /// </summary>
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICopiaRepository _copiaRepository;
        private readonly ILogger<PrestamoService> _logger;

        /// <summary>
        /// Constructor que inyecta los repositorios y el logger necesarios para
        /// manejar la lógica de préstamos.
        /// </summary>
        public PrestamoService(
            IPrestamoRepository prestamoRepository,
            IClienteRepository clienteRepository,
            ICopiaRepository copiaRepository,
            ILogger<PrestamoService> logger)
        {
            _prestamoRepository = prestamoRepository;
            _clienteRepository = clienteRepository;
            _copiaRepository = copiaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retorna la lista de todos los Alquileres registrados, incluyendo información
        /// de cliente, detalles y copias asociadas. Utiliza el método ObtenerPrestamosAsync()
        /// del repositorio de préstamos.
        /// </summary>
        /// <returns>Colección de objetos Alquiler</returns>
        public async Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync()
        {
            _logger.LogInformation("Obteniendo lista de préstamos (alquileres)...");
            return await _prestamoRepository.ObtenerPrestamosAsync();
        }

        /// <summary>
        /// Registra un nuevo préstamo (Alquiler), validando:
        /// 1. Que el cliente no esté en lista negra.
        /// 2. Que no exceda el máximo de 3 copias activas.
        /// 3. Que las copias solicitadas existan y estén en estado "Disponible".
        /// 
        /// Si todo es correcto, crea la entidad Alquiler y la persiste con el repositorio,
        /// asignando estado "Prestado" a las copias.
        /// </summary>
        /// <param name="clienteId">ID del cliente solicitante</param>
        /// <param name="copiasId">Lista de IDs de copias a prestar</param>
        /// <param name="fechaFin">Fecha límite de devolución del préstamo</param>
        /// <returns>La entidad Alquiler creada, con su nuevo ID y datos asociados</returns>
        public async Task<Alquiler> RegistrarPrestamoAsync(int clienteId, List<int> copiasId, DateTime fechaFin)
        {
            _logger.LogInformation("Registrando préstamo para cliente {ClienteId} con copias {Copias}",
                clienteId, string.Join(",", copiasId));

            // 1) Validar si el cliente está en lista negra
            bool enListaNegra = await _clienteRepository.EstaEnListaNegraAsync(clienteId);
            if (enListaNegra)
            {
                _logger.LogWarning("Cliente {ClienteId} está en lista negra.", clienteId);
                throw new Exception("El cliente está en lista negra.");
            }

            // 2) Validar máximo de 3 copias
            int copiasActivas = await _prestamoRepository.ObtenerCopiasActivasDeClienteAsync(clienteId);
            if (copiasActivas + copiasId.Count > 3)
            {
                _logger.LogWarning("Cliente {ClienteId} excede el límite de 3 copias.", clienteId);
                throw new Exception("El cliente ya superó el límite de 3 copias en préstamo.");
            }

            // 3) Verificar disponibilidad de copias
            var copiasSolicitadas = await _copiaRepository.ObtenerCopiasPorIdsAsync(copiasId);
            if (copiasSolicitadas.Count != copiasId.Count)
                throw new Exception("Alguna de las copias solicitadas no existe en la base de datos.");

            foreach (var copia in copiasSolicitadas)
            {
                // Validar que esté "Disponible"
                if (copia.Estado != CopiaEstado.Disponible)
                {
                    _logger.LogWarning("Copia {CopiaId} con estado {Estado}, no se puede prestar.",
                        copia.CopiaId, copia.Estado);
                    throw new Exception($"La copia '{copia.CodigoBarras}' no está disponible (actual: {copia.Estado}).");
                }
            }

            // 4) Crear la entidad Alquiler
            var alquiler = new Alquiler
            {
                ClienteId = clienteId,
                FechaInicio = DateTime.Now,
                FechaFin = fechaFin,
                Penalidad = 0
            };

            // 5) Persiste el Alquiler y detalla las copias
            await _prestamoRepository.RegistrarAlquilerAsync(alquiler, copiasId);

            _logger.LogInformation("Préstamo registrado con AlquilerId={AlquilerId}", alquiler.AlquilerId);
            return alquiler;
        }
    }
}
