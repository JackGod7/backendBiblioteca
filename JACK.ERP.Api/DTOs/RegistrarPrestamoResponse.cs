// JACK.ERP.Api\DTOs\RegistrarPrestamoResponse.cs
using System;

namespace JACK.ERP.Api.DTOs
{
    public class RegistrarPrestamoResponse
    {
        public int ReservaId { get; set; }
        public DateTime FechaReserva { get; set; }
        public string Estado { get; set; }
        public string Mensaje { get; set; }
    }
}
