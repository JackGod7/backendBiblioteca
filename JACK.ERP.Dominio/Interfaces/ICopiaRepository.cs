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
        // Devuelve la copia por su ID
        Task<Copia> ObtenerCopiaAsync(int copiaId);

        // O si prefieres un método para validar múltiples copias a la vez:
        Task<List<Copia>> ObtenerCopiasPorIdsAsync(IEnumerable<int> copiasIds);
    }
}
