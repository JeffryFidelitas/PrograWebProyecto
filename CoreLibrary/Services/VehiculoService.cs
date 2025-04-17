using CoreLibrary.Auth;
using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class VehiculoService : IVehiculoService
    {
    
        private readonly ProyectDBContext _context;

        public VehiculoService(ProyectDBContext context)
        {
            _context = context;
        }


        public async Task CrearAsync(Vehiculo vehiculo)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Vehiculos.Add(vehiculo);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocurrió un error al registrar el vehiculo.", ex);
            }
        }

        public async Task<Vehiculo> ObtenerPorPlacaAsync(string placa)
        {
            try
            {
                return await _context.Vehiculos.FirstAsync(v => v.Placa == placa);
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrio un error obteniendo el vehiculo por placa.", ex);
            }
        }
    }
}
