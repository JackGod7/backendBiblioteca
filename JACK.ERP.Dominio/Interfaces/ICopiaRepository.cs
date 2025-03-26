using JACK.ERP.Dominio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Interfaces
{
    public interface ICopiaRepository
    {

        // O si prefieres un método para validar múltiples copias a la vez:
        Task<List<Copia>> ObtenerCopiasPorIdsAsync(IEnumerable<int> copiasIds);
    }
}
