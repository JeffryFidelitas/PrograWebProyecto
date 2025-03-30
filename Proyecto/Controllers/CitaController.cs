using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;

namespace Proyecto.Controllers;

public class CitaController : Controller
{
    private readonly ProyectoContext _context;
    public CitaController(ProyectoContext context)
    {
        _context = context;
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create([Bind("TipoAuto", "Fecha", "Hora")] Cita cita)
    {
        return View(cita);
    }

    [HttpGet]
    public JsonResult AvailableTimes(string date)
    {
        if (DateOnly.TryParse(date, out DateOnly parsedDate))
        {
            List<string> AvailableDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
            List<string> AvailableTimes = ["7:00 AM", "8:00 AM", "9:00 AM", "10:00 AM", "11:00 AM", "12:00 PM", "1:00 PM", "2:00 PM", "3:00 PM", "4:00 PM"];
            
            if (!AvailableDays.Contains(parsedDate.DayOfWeek.ToString()))
                return Json(new { times = (List<string>) []}); 
                       
            List<Cita> CitasExistentes = _context.Citas.Where(c => c.Fecha == parsedDate).ToList();

            foreach (Cita cita in CitasExistentes)
                AvailableTimes.Remove(cita.Hora.ToString("h:mm tt"));

            return Json(new { times = AvailableTimes });
        }
        return Json(new { times = (List<string>) [] });
    }
}