using System.Diagnostics;
using System.Security.Claims;
using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Controllers;

public class HomeController : Controller
{
    private readonly IHomeService _homeService;
    private readonly IUsuarioService _usuarioService;

    public HomeController(IHomeService homeService, IUsuarioService usuarioService)
    {
        _homeService = homeService;
        _usuarioService = usuarioService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
                {
                    return RedirectToAction("Error", "Home");
                }

                var usuario = await _usuarioService.ObtenerPorIdAsync(usuarioId);

                if (usuario == null)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Login", "Usuarios");
                }

                var modelo = await _homeService.ObtenerIndexParaUsuario(usuario);
                return View(modelo);
            }

            // Si no está autenticado, solo mostramos los servicios activos
            var modeloInvitado = await _homeService.ObtenerIndexParaInvitado();
            return View(modeloInvitado);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

}
