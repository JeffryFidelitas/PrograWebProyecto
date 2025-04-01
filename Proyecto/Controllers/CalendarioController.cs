using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Proyecto.Models;

namespace Proyecto.Controllers;

public class CalendarioController : Controller
{
    private readonly ProyectoContext _context;
    public CalendarioController(ProyectoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public JsonResult Calendario()
    {
        var events = new List<Dictionary<string, object>> {};

        List<Cita> citas = _context.Citas.Include(c => c.TipoLavado).Include(c => c.usuario).ToList();

        foreach (Cita cita in citas)
        {
            if (cita.TipoLavado != null && cita.usuario != null)
            {
                DateTime StartDateTime = cita.Fecha.ToDateTime(cita.Hora);
                DateTime EndDateTime = StartDateTime.AddMinutes(cita.TipoLavado.Duracion);

                events.Add(new Dictionary<string,object>{
                    { "id", cita.Id.ToString() },
                    { "title", cita.TipoAuto.ToString() + " " + cita.TipoLavado.Nombre  + " (" + cita.usuario.Email + ")"},
                    { "start", StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "end", EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "editable", false },
                    { "url", "/Cita/Details/" + cita.Id.ToString() },
                    { "backgroundColor", cita.Realizada ? "#198754" : "#3788D8" }
                });
            }
        }

        return Json(events);
    }
}