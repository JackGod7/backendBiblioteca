using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Entities
{
    public class Alquiler
    {
        public int AlquilerId { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public decimal Penalidad { get; set; }

        public Cliente Cliente { get; set; }
        public ICollection<AlquilerDetalle> Detalles { get; set; }
    }
}
