// JACK.ERP.Aplicacion\Services\Entidades\PrestamoService.cs
using JACK.ERP.Aplicacion.Interfaces.Entidades;
using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using Microsoft.Extensions.Logging; // para logs


namespace JACK.ERP.Aplicacion.Services.Entidades
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICopiaRepository _copiaRepository;
        private readonly ILogger<PrestamoService> _logger;

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

        public async Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync()
        {
            _logger.LogInformation("Obteniendo lista de préstamos (alquileres)...");
            return await _prestamoRepository.ObtenerPrestamosAsync();
        }

        public async Task<Alquiler> RegistrarPrestamoAsync(int clienteId, List<int> copiasId, DateTime fechaFin)
        {
            _logger.LogInformation("Registrando préstamo para cliente {ClienteId} con copias {Copias}",
                clienteId, string.Join(",", copiasId));

            // 1) Validar lista negra
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

            // 3) Verificar disponibilidad
            var copiasSolicitadas = await _copiaRepository.ObtenerCopiasPorIdsAsync(copiasId);
            if (copiasSolicitadas.Count != copiasId.Count)
                throw new Exception("Alguna de las copias solicitadas no existe en la base de datos.");

            foreach (var copia in copiasSolicitadas)
            {
                // Asumiendo que copia.Estado es un enum
                if (copia.Estado != CopiaEstado.Disponible)
                {
                    _logger.LogWarning("Copia {CopiaId} con estado {Estado}, no se puede prestar.",
                        copia.CopiaId, copia.Estado);
                    throw new Exception($"La copia '{copia.CodigoBarras}' no está disponible (actual: {copia.Estado}).");
                }
            }

            // 4) Crear Alquiler
            var alquiler = new Alquiler
            {
                ClienteId = clienteId,
                FechaInicio = DateTime.Now,
                FechaFin = fechaFin,
                Penalidad = 0
            };

            // 5) Persistir con repositorio
            await _prestamoRepository.RegistrarAlquilerAsync(alquiler, copiasId);

            _logger.LogInformation("Préstamo registrado con AlquilerId={AlquilerId}", alquiler.AlquilerId);
            return alquiler;
        }
    }
}
