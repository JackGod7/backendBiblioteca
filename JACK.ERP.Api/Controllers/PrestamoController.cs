using JACK.ERP.Api.DTOs;
using JACK.ERP.Aplicacion.Interfaces.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JACK.ERP.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamoController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;

        // Inyección de dependencia correcta
        public PrestamoController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarPrestamo([FromBody] RegistrarPrestamoRequest request)
        {
            try
            {
                var alquilerCreado = await _prestamoService.RegistrarPrestamoAsync(
                    request.ClienteId,
                    request.CopiasId,
                    request.FechaFin
                );

                return Ok(new
                {
                    reservaId = alquilerCreado.AlquilerId,
                    fechaReserva = alquilerCreado.FechaInicio,
                    estado = "Pendiente",
                    mensaje = "Préstamo registrado correctamente"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpGet("listar")]
        public async Task<IActionResult> ListarPrestamosAsync()
        {
            try
            {
                var prestamos = await _prestamoService.ObtenerPrestamosAsync();
                return Ok(prestamos);
            }
            catch (Exception ex)
            {
                // En lugar de relanzar la excepción, lo normal es devolver un 500 con el mensaje
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
