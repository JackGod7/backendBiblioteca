namespace JACK.ERP.Api.DTOs
{
    public class RegistrarPrestamoRequest
    {
        public int ClienteId { get; set; }
        public List<int> CopiasId { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
