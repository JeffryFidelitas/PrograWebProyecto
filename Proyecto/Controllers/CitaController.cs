using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Proyecto.Models;

namespace Proyecto.Controllers;

public class CitaController : Controller
{
    private readonly ProyectoContext _context;
    public CitaController(ProyectoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        Usuario? usuario = _context.Usuarios.Find(1); //TODO Cambiar por usuario logeado
        ViewData["UsuarioLogeado"] = usuario;
        IEnumerable<Cita> citas = _context.Citas
            .Include(c => c.TipoLavado)
            .Where(c => c.usuario.Id == usuario.Id || usuario.Rol == Roles.Administrador)
            .ToImmutableArray();
        return View(citas);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["TiposLavado"] = _context.Lavado.Where(t => t.Estado).ToImmutableArray();
        return View();
    }

    [HttpPost]
    public IActionResult Create([Bind("TipoAuto", "Fecha", "Hora")] Cita cita, int TipoLavado)
    {
        cita.TipoLavado = _context.Lavado.Find(TipoLavado);
        cita.usuario = _context.Usuarios.Find(1); //TODO: Cambiar por usuario logeado
        _context.Citas.Add(cita);
        _context.SaveChanges();
        return View();
    }

    [HttpPost]
    public IActionResult ToggleRealizada(int Id, int Realizada)
    {
        Cita cita = _context.Citas.Find(Id);
        if (cita != null)
        {
            cita.Realizada = Realizada == 1;
            _context.Update(cita);
            _context.SaveChanges();
        }
        //Teoricamente aca se manda un correo
        return RedirectToAction("Index");
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