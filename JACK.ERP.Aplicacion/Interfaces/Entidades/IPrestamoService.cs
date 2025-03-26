using JACK.ERP.Dominio.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace JACK.ERP.Aplicacion.Interfaces.Entidades
{
    /// <summary>
    /// Define la API de la capa de aplicación para gestionar préstamos (Alquiler).
    /// Contiene métodos para listar y registrar préstamos, aplicando las reglas de negocio.
    /// </summary>
    public interface IPrestamoService
    {
        /// <summary>
        /// Obtiene todos los Alquileres existentes, con detalles y cliente asociado.
        /// Útil para mostrar el historial o estado actual de los préstamos.
        /// </summary>
        /// <returns>Lista de entidades Alquiler</returns>
        Task<IEnumerable<Alquiler>> ObtenerPrestamosAsync();

        /// <summary>
        /// Registra un nuevo préstamo para un cliente, validando reglas de negocio:
        /// lista negra, máximo de copias activas y disponibilidad de cada copia.
        /// Retorna la entidad Alquiler creada.
        /// </summary>
        /// <param name="clienteId">ID del cliente solicitante</param>
        /// <param name="copiasId">Lista de IDs de copias a prestar</param>
        /// <param name="fechaFin">Fecha límite de devolución</param>
        /// <returns>Entidad Alquiler con los datos del nuevo préstamo</returns>
        Task<Alquiler> RegistrarPrestamoAsync(int clienteId, List<int> copiasId, DateTime fechaFin);
    }
}
