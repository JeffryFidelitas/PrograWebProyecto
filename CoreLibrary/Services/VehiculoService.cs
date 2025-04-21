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

        public async Task<(bool Success, string Message)> AgregarVehiculoAsync(Vehiculo vehiculo, Cliente cliente)
        {
            if (cliente == null)
            {
                return (false, "Cliente inválido.");
            }

            var existe = await _context.Vehiculos
                .AnyAsync(v => v.Placa == vehiculo.Placa && v.ClienteId == cliente.Id);

            if (existe)
            {
                return (false, "Ya existe un vehículo con esa placa registrado para este cliente.");
            }

            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            return (true, "Vehículo agregado exitosamente.");
        }

        public async Task<(bool Success, string Message)> EditarVehiculoAsync(Vehiculo vehiculo, Cliente cliente)
        {
            if (cliente == null)
            {
                return (false, "Cliente inválido.");
            }
            var vehiculoExistente = await _context.Vehiculos
                .FirstOrDefaultAsync(v => v.Id == vehiculo.Id && v.ClienteId == cliente.Id);
            if (vehiculoExistente == null)
            {
                return (false, "Vehículo no encontrado.");
            }

            vehiculoExistente.Marca = vehiculo.Marca;
            vehiculoExistente.Modelo = vehiculo.Modelo;
            vehiculoExistente.Color = vehiculo.Color;
            vehiculoExistente.Placa = vehiculo.Placa;
            vehiculoExistente.Tipo = vehiculo.Tipo;

            await _context.SaveChangesAsync();
            return (true, "Vehículo editado exitosamente.");
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
