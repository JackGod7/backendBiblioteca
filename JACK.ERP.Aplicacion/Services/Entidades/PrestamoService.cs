using JACK.ERP.Aplicacion.Interfaces.Entidades;
using JACK.ERP.Dominio.Entities;
using JACK.ERP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JACK.ERP.Aplicacion.Services.Entidades
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IPrestamoRepository _prestamoRepository;
        private readonly IClienteRepository _clienteRepository; // Para lista negra
        private readonly ICopiaRepository _copiaRepository;
        public PrestamoService(
            IPrestamoRepository prestamoRepository,
            IClienteRepository clienteRepository,
            ICopiaRepository copiaRepository)
        {
            _prestamoRepository = prestamoRepository;
            _clienteRepository = clienteRepository;
            _copiaRepository = copiaRepository;
        }

        public async Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync()
        {
            return await _prestamoRepository.ObtenerPrestamosAsync();
        }

        // Nuevo método para registrar un préstamo
        public async Task<Alquiler> RegistrarPrestamoAsync(int clienteId, List<int> copiasId, DateTime fechaFin)
        {
            // 1) Verificar si el cliente está en lista negra
            bool enListaNegra = await _clienteRepository.EstaEnListaNegraAsync(clienteId);
            if (enListaNegra)
                throw new Exception("El cliente está en lista negra y no puede prestar libros.");

            // 2) Contar cuántas copias aún tiene sin devolver
            int copiasActivas = await _prestamoRepository.ObtenerCopiasActivasDeClienteAsync(clienteId);
            if (copiasActivas + copiasId.Count > 3)
                throw new Exception("El cliente ya superó el límite de 3 copias en préstamo.");

            // 3) Verificar si cada copia existe y está “Disponible”
            var copiasSolicitadas = await _copiaRepository.ObtenerCopiasPorIdsAsync(copiasId);
            if (copiasSolicitadas.Count != copiasId.Count)
                throw new Exception("Algunas de las copias solicitadas no existen.");

            foreach (var copia in copiasSolicitadas)
            {
                if (copia.Estado != "Disponible")
                    throw new Exception($"La copia '{copia.CodigoBarras}' no está en estado disponible (actual: {copia.Estado}).");
            }

            // 4) Crear la entidad Alquiler
            var alquiler = new Alquiler
            {
                ClienteId = clienteId,
                FechaInicio = DateTime.Now,
                FechaFin = fechaFin,
                Penalidad = 0
            };

            // 5) Guardar usando PrestamoRepository (transacción + actualización de estado)
            await _prestamoRepository.RegistrarAlquilerAsync(alquiler, copiasId);

            // 6) Devolver el alquiler creado
            return alquiler;
        }
    }
}
