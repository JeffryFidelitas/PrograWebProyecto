using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using CoreLibrary.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using CoreLibrary.Services.Interfaces;

namespace Proyecto.Controllers;

public class ServicioController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IServicioService _servicioService;
    private readonly IUsuarioService _usuarioService;

    public ServicioController(ICitaService citaService, IServicioService servicioService, IUsuarioService usuarioService)
    {
        _citaService = citaService;
        _servicioService = servicioService;
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(usuarioId);

            if (usuario == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Usuarios");
            }

            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion,Precio,Duracion")] Servicio servicio)
    {
        try
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(usuarioId);

            if (usuario == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Usuarios");
            }
            await _servicioService.CrearAsync(servicio);
            return View(servicio);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}