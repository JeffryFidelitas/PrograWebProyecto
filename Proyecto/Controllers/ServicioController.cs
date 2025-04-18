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
    public async Task<IActionResult> Index()
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

            // Obtener todos los servicios
            var servicios = await _servicioService.ObtenerTodosAsync();

            return View(servicios);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
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
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
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

            var servicio = await _servicioService.ObtenerPorIdAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Duracion,Activo")] Servicio servicio)
    {
        try
        {
            // Verificar que el ID coincide
            if (id != servicio.Id)
            {
                return NotFound();
            }

            // Verificar autenticación
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

            // Obtener el servicio original para asegurarnos que existe
            var servicioOriginal = await _servicioService.ObtenerPorIdAsync(id);
            if (servicioOriginal == null)
            {
                return NotFound();
            }

            // Actualizar las propiedades
            servicioOriginal.Nombre = servicio.Nombre;
            servicioOriginal.Descripcion = servicio.Descripcion;
            servicioOriginal.Precio = servicio.Precio;
            servicioOriginal.Duracion = servicio.Duracion;
            servicioOriginal.Activo = servicio.Activo;

            // Guardar los cambios
            await _servicioService.ActualizarAsync(servicioOriginal);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}