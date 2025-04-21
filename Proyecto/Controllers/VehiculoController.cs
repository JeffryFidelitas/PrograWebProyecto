using CoreLibrary.Auth;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Services.Interfaces;

namespace Proyecto.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly IUsuarioService _usuarioService;

        public VehiculoController(IVehiculoService vehiculoService, IUsuarioService usuarioService)
        {
            _vehiculoService = vehiculoService;
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpPost] // Funcional
        public async Task<IActionResult> NuevoVehiculo([FromForm] ViewModel_Vehiculo viewModel)
        {
            var usuarioId = RoleHelper.ObtenerId(User);
            if (usuarioId == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos del vehículo inválidos." });
            }

            var cliente = await _usuarioService.ObtenerClientePorIdAsync(usuarioId.Value);
            if (cliente == null)
            {
                return Json(new { success = false, message = "Cliente no encontrado." });
            }

            var vehiculo = new Vehiculo
            {
                Marca = viewModel.Marca,
                Modelo = viewModel.Modelo,
                Color = viewModel.Color,
                Placa = viewModel.Placa,
                Tipo = viewModel.Tipo,
                ClienteId = cliente.Id
            };

            var resultado = await _vehiculoService.AgregarVehiculoAsync(vehiculo, cliente);
            if (!resultado.Success)
            {
                return Json(new { success = false, message = resultado.Message });
            }

            return Json(new { success = true, message = "Vehículo agregado correctamente." });
        }

        // Editar Vehículo
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditarVehiculo([FromForm] ViewModel_Vehiculo viewModel)
        {
            var usuarioId = RoleHelper.ObtenerId(User);
            if (usuarioId == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos del vehículo inválidos." });
            }
            var cliente = await _usuarioService.ObtenerClientePorIdAsync(usuarioId.Value);
            if (cliente == null)
            {
                return Json(new { success = false, message = "Cliente no encontrado." });
            }
            var vehiculo = new Vehiculo
            {
                Id = viewModel.Id,
                Marca = viewModel.Marca,
                Modelo = viewModel.Modelo,
                Color = viewModel.Color,
                Placa = viewModel.Placa,
                Tipo = viewModel.Tipo,
                ClienteId = cliente.Id
            };
            var resultado = await _vehiculoService.EditarVehiculoAsync(vehiculo, cliente);
            if (!resultado.Success)
            {
                return Json(new { success = false, message = resultado.Message });
            }
            return Json(new { success = true, message = "Vehículo editado correctamente." });
        }
    }
}
