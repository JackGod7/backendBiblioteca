using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Entities
{
    public class AlquilerDetalle
    {
        public int DetalleId { get; set; }
        public int AlquilerId { get; set; }
        public int CopiaId { get; set; }

        [JsonIgnore]
        public Alquiler Alquiler { get; set; }
        public Copia Copia { get; set; }
    }
}
