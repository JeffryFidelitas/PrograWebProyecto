using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services.Interfaces
{
    public interface IVehiculoService
    {
        Task CrearAsync(Vehiculo usuario);
        Task<Vehiculo> ObtenerPorPlacaAsync(string placa);
        Task<(bool Success, string Message)> AgregarVehiculoAsync(Vehiculo vehiculo, Cliente cliente);
        Task<(bool Success, string Message)> EditarVehiculoAsync(Vehiculo vehiculo, Cliente cliente);
    }
}
