using JACK.ERP.Aplicacion.Interfaces.Entidades;
using JACK.ERP.Api.DTOs;
using JACK.ERP.Api.Mappings;  // Clase con metodo de extensión ToListarPrestamoResponse
using JACK.ERP.Dominio.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JACK.ERP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;

        public PrestamoController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        // GET: /api/Prestamo/listar
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<ListarPrestamoResponse>>> ListarPrestamosAsync()
        {
            var alquileres = await _prestamoService.ObtenerPrestamosAsync();
            var response = alquileres
                .Select(a => a.ToListarPrestamoResponse());

            // Retornamos un 200 OK con el array
            return Ok(response);
        }

        // POST: /api/Prestamo/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<RegistrarPrestamoResponse>> RegistrarPrestamo([FromBody] RegistrarPrestamoRequest request)
        {
            var alquilerCreado = await _prestamoService.RegistrarPrestamoAsync(
                request.ClienteId,
                request.CopiasId,
                request.FechaFin
            );

            var response = new RegistrarPrestamoResponse
            {
                ReservaId = alquilerCreado.AlquilerId,
                FechaReserva = alquilerCreado.FechaInicio,
                Estado = "Pendiente",
                Mensaje = "Préstamo registrado correctamente"
            };

            return Ok(response);
        }
    }
}
