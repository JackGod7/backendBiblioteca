using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Entities
{
    public class ListaNegra
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public Cliente Cliente { get; set; }
    }
}
