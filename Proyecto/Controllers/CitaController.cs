using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Proyecto.Controllers;

public class CitaController : Controller
{
    private readonly ICitaService _citaService;

    public CitaController(ICitaService citaService)
    {
        _citaService = citaService;
    }

    [HttpGet]
    public async Task<IActionResult> CrearCita(Cita cita)
    {
        try
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            var usuario = await _citaService.ObtenerPorIdAsync(usuarioId);

            if (usuario == null)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Usuarios");
            }

            return View(cita);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }


    //Solo Cliente
    [HttpGet]
    public JsonResult AvailableTimes(string date)
    {
        //if (DateOnly.TryParse(date, out DateOnly parsedDate))
        //{
        //    List<string> AvailableDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
        //    List<string> AvailableTimes = ["7:00 AM", "8:00 AM", "9:00 AM", "10:00 AM", "11:00 AM", "12:00 PM", "1:00 PM", "2:00 PM", "3:00 PM", "4:00 PM"];
            
        //    if (!AvailableDays.Contains(parsedDate.DayOfWeek.ToString()))
        //        return Json(new { times = (List<string>) []}); 
                       
        //    List<Cita> CitasExistentes = _context.Citas.Where(c => c.Fecha == parsedDate).ToList();

        //    foreach (Cita cita in CitasExistentes)
        //        AvailableTimes.Remove(cita.Hora.ToString("h:mm tt"));

        //    return Json(new { times = AvailableTimes });
        //}
        return Json(new { times = (List<string>) [] });
    }

}