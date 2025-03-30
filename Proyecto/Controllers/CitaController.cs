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

    [HttpGet]
    public JsonResult AvailableTimes(string date)
    {
        if (DateTime.TryParse(date, out DateTime parsedDate))
        {
            List<string> AvailableTimes = ["7:00 AM", "8:00 AM", "9:00 AM", "10:00 AM", "11:00 AM", "12:00 PM", "1:00 PM", "2:00 PM", "3:00 PM", "4:00 PM"];
            List<Cita> existing_citas = _context.Citas.Where(c => c.FechaHora.Date == parsedDate.Date).ToList();

            foreach (Cita cita in existing_citas)
                AvailableTimes.Remove(cita.FechaHora.ToString("h:mm tt"));

            return Json(new { times = AvailableTimes });
        }
        return Json(new { times = (List<string>) [] });
    }
}