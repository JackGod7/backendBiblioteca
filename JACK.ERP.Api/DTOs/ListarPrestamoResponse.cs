
namespace JACK.ERP.Api.DTOs
{
    public class ListarPrestamoResponse
    {
        public int AlquilerId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public decimal Penalidad { get; set; }
        public List<DetallePrestamoDTO> Detalles { get; set; }
    }

    public class DetallePrestamoDTO
    {
        public int DetalleId { get; set; }
        public int CopiaId { get; set; }
        public string CodigoBarras { get; set; }
        public string Estado { get; set; }
    }
}
