using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombres { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
    }
}
