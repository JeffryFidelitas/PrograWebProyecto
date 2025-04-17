using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using CoreLibrary.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using CoreLibrary.Services.Interfaces;
using CoreLibrary.Auth;

namespace Proyecto.Controllers;

public class CitaController : Controller
{
    private readonly ICitaService _citaService;
    private readonly IServicioService _servicioService;
    private readonly IUsuarioService _usuarioService;
    private readonly IVehiculoService _vehiculoService;

    public CitaController(ICitaService citaService, IServicioService servicioService, IUsuarioService usuarioService, IVehiculoService vehiculoService)
    {
        _citaService = citaService;
        _servicioService = servicioService;
        _usuarioService = usuarioService;
        _vehiculoService = vehiculoService;
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

            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<JsonResult> Calendario()
    {
        Dictionary<string, string> colores = new Dictionary<string, string>()
        {
            { "Pendiente", "#CCA700" },
            { "Confirmada", "#AFCC9B" },
            { "Cancelada", "#7E2129" },
            { "Completada", "#179FFF" },
        };

        var events = new List<Dictionary<string, object>> {};

        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Json("Error");
        }

        Usuario? usuario = await _usuarioService.ObtenerPorIdAsync(usuarioId);

        if (usuario == null)
        {
            return Json("Error");
        }

        IEnumerable<Cita> citas = await _citaService.ObtenerTodasAsync(usuario.Id, usuario.Rol == Roles.Administrador);

        foreach (Cita cita in citas)
        {
            DateTime StartDateTime = cita.FechaHora;

            int duracion = 0;
            foreach (CitaServicio servicio in cita.CitasServicios)
            {
                duracion += servicio.Servicio.Duracion;
            }

            DateTime EndDateTime = StartDateTime.AddMinutes(duracion);
            if (cita.CitasServicios.Count > 0)
            {
                events.Add(new Dictionary<string, object>{
                    { "id", cita.Id.ToString() },
                    { "title", cita.Vehiculo.Cliente.NombreCompleto + ", " + cita.CitasServicios.First().Servicio.Nombre},
                    { "start", StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "end", EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss") },
                    { "editable", false },
                    { "url", "/Cita/Details/" + cita.Id.ToString() },
                    { "backgroundColor", colores[cita.Estado.ToString()] }
                });
            }
        }

        return Json(events);
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

            ViewData["Servicios"] = await _servicioService.ObtenerTodosActivosAsync();

            return View();
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(string VehiculoTipo, string VehiculoPlaca, string VehiculoMarca, string VehiculoModelo, string VehiculoColor, int ServicioId, DateOnly Fecha, TimeOnly Hora)
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

            ViewData["Servicios"] = await _servicioService.ObtenerTodosActivosAsync();

            Cliente cliente = await _usuarioService.ObtenerClientePorIdAsync(usuarioId);

            Vehiculo vehiculo = await _vehiculoService.ObtenerPorPlacaAsync(VehiculoPlaca);
            if (vehiculo == null)
            {
                vehiculo = new Vehiculo();
                vehiculo.Placa = VehiculoPlaca;
                vehiculo.Tipo = VehiculoTipo;
                vehiculo.Marca = VehiculoMarca;
                vehiculo.Modelo = VehiculoModelo;
                vehiculo.Color = VehiculoColor;
                vehiculo.ClienteId = usuarioId;
                vehiculo.Cliente = cliente;
                await _vehiculoService.CrearAsync(vehiculo);
            }
            
            Cita cita = new Cita();
            cita.ClienteId = usuario.Id;
            cita.VehiculoId = vehiculo.Id;
            cita.FechaHora = Fecha.ToDateTime(Hora);
            cita.Estado = EstadoCita.Pendiente;
            cita.Cliente = cliente;
            cita.Vehiculo = vehiculo;
            cita.CitasServicios = [];

            await _citaService.CrearAsync(cita);

            CitaServicio citaServicio = new CitaServicio();
            citaServicio.Cita = cita;
            citaServicio.CitaId = cita.Id;
            citaServicio.Servicio = await _servicioService.ObtenerPorIdAsync(ServicioId);
            citaServicio.ServicioId = ServicioId;

            cita.CitasServicios.Add(citaServicio);

            await _citaService.ActualizarAsync(cita);

            return View(cita);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CambiarEstado(int Id, string Estado)
    {
        Cita? cita = await _citaService.ObtenerPorIdAsync(Id);
        if (cita != null)
        {
            cita.Estado = (EstadoCita) Enum.Parse(typeof(EstadoCita), Estado);
            await _citaService.ActualizarAsync(cita);
        }
        return RedirectToAction("Details", "Cita", new {id = Id});
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        return View(await _citaService.ObtenerPorIdAsync(id));
    }

    //Solo Cliente
    [HttpGet]
    public async Task<JsonResult> AvailableTimes(string date)
    {
        if (DateOnly.TryParse(date, out DateOnly parsedDate))
        {
            List<string> AvailableDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
            List<string> AvailableTimes = ["7:00 AM", "8:00 AM", "9:00 AM", "10:00 AM", "11:00 AM", "12:00 PM", "1:00 PM", "2:00 PM", "3:00 PM", "4:00 PM"];
            
            if (!AvailableDays.Contains(parsedDate.DayOfWeek.ToString()))
                return Json(new { times = (List<string>) []}); 
                       
            List<Cita> CitasExistentes = (await _citaService.ObtenerTodasAsync(-1, true)).Where(c => DateOnly.FromDateTime(c.FechaHora) == parsedDate).ToList();

            foreach (Cita cita in CitasExistentes)
                AvailableTimes.Remove(cita.FechaHora.ToString("h:mm tt"));

            return Json(new { times = AvailableTimes });
        }
        return Json(new { times = (List<string>) [] });
    }

}