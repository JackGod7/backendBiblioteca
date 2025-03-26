using JACK.ERP.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Interfaces
{
    /// <summary>
    /// Define las operaciones de acceso a datos relacionadas con la entidad Copia.
    /// Se utiliza principalmente para obtener y validar disponibilidad de copias.
    /// </summary>
    public interface ICopiaRepository
    {
        /// <summary>
        /// Obtiene todas las copias con IDs contenidos en <paramref name="copiasIds"/>.
        /// Permite validar disponibilidad de cada copia y/o cargar sus datos.
        /// </summary>
        /// <param name="copiasIds">Lista de IDs de las copias requeridas</param>
        /// <returns>Lista de copias correspondientes a los IDs solicitados</returns>
        Task<List<Copia>> ObtenerCopiasPorIdsAsync(IEnumerable<int> copiasIds);
    }
}
