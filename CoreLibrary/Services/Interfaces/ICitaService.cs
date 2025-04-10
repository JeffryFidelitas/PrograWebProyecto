using CoreLibrary.Models;

namespace CoreLibrary.Services.Interfaces
{
    public interface ICitaService
    {
        Task<IEnumerable<Cita>> ObtenerTodasAsync(int usuarioId, bool esAdmin);
        Task<Cita?> ObtenerPorIdAsync(int id);
        Task CrearAsync(Cita cita);
        Task ActualizarAsync(Cita cita);
        Task EliminarAsync(int id);
    }
}
