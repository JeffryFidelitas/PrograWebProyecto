using CoreLibrary.Models;

namespace CoreLibrary.Services.Interfaces
{
    public interface IServicioService
    {
        Task<List<Servicio>> ObtenerTodosActivosAsync();
        Task<List<Servicio>> ObtenerTodosAsync();
        Task<Servicio?> ObtenerPorIdAsync(int id);
        Task<Servicio> CrearAsync(Servicio servicio);
        Task<bool> ActualizarAsync(Servicio servicio);
        Task<bool> EliminarAsync(int id);
        Task<bool> ReactivarAsync(int id);
    }
}
