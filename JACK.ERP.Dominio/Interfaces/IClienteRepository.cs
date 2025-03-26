using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JACK.ERP.Dominio.Interfaces
{
    public interface IClienteRepository
    {
        Task<bool> EstaEnListaNegraAsync(int clienteId);
    }
}
