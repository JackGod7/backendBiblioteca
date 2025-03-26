// JACK.ERP.Api\Mappings\PrestamoMapping.cs
using JACK.ERP.Api.DTOs;
using JACK.ERP.Dominio.Entities;
using System.Collections.Generic;
using System.Linq;

namespace JACK.ERP.Api.Mappings
{
    public static class PrestamoMapping
    {
        public static ListarPrestamoResponse ToListarPrestamoResponse(this Alquiler alquiler)
        {
            return new ListarPrestamoResponse
            {
                AlquilerId = alquiler.AlquilerId,
                ClienteId = alquiler.ClienteId,
                NombreCliente = alquiler.Cliente?.Nombres,
                FechaInicio = alquiler.FechaInicio,
                FechaFin = alquiler.FechaFin,
                FechaDevolucion = alquiler.FechaDevolucion,
                Penalidad = alquiler.Penalidad,
                Detalles = alquiler.Detalles?.Select(d => new DetallePrestamoDTO
                {
                    DetalleId = d.DetalleId,
                    CopiaId = d.CopiaId,
                    CodigoBarras = d.Copia?.CodigoBarras,
                    Estado = d.Copia?.Estado.ToString()
                }).ToList() ?? new List<DetallePrestamoDTO>()
            };
        }
    }
}
