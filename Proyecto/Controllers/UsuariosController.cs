using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using CoreLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using CoreLibrary.Auth;
using CoreLibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace Proyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ProyectDBContext _context;

        public UsuariosController(IUsuarioService usuarioService, ProyectDBContext context)
        {
            _usuarioService = usuarioService;
            _context = context;

        }

        #region Autenticacion
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Correo,Contrasenna")] ViewModel_UsuarioLogin usuario)
        {

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var usuarioExistente = await _usuarioService.BuscarPorCredenciales(usuario.Correo, usuario.Contrasenna);

            if (usuarioExistente == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(usuario);
            }

            // Crear los claims y autenticar
            await AutenticarUsuario(usuarioExistente);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Nombre,Apellido,Telefono,Correo,Contrasenna")] ViewModel_UsuarioRegister usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }
            // Verificar si el usuario ya existe
            var usuarioExistente = await _usuarioService.BuscarPorCorreo(usuario.Correo);

            if (usuarioExistente != null)
            {
                ModelState.AddModelError(string.Empty, "El usuario ya existe.");
                return View(usuario);
            }

            await _usuarioService.CrearAsync(usuario);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Informacion de Usuario
        [HttpGet]
        public async Task<IActionResult> PerfilUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPerfilPorIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            // Verifica si el usuario autenticado es el mismo que el que se está editando
            if (User.FindFirst(ClaimTypes.NameIdentifier)?.Value != id.ToString())
            {
                return Forbid(); // O redirigir a una página de error
            }

            return View(usuario);
        }
        #endregion

        #region Metodos Privados
        // Método separado para manejar los claims y autenticación
        private async Task AutenticarUsuario(Usuario usuario)
        {
            var claims = ObtenerClaims(usuario);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        // Método para generar los claims
        private List<Claim> ObtenerClaims(Usuario usuario)
        {
            return new List<Claim>
                {
                    // Aca se pueden agregar más claims según sea necesarios
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()!),
                    new Claim(ClaimTypes.Email, usuario.Correo!),
                    new Claim(ClaimTypes.Name, usuario.Nombre!),
                    new Claim(ClaimTypes.Surname, usuario.Apellido!),
                    new Claim(ClaimTypes.Role, usuario.Rol.ToString()!)
                };
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListarUsuarios()
        {
            // Obtener el usuario actual
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var usuarioActual = await _usuarioService.ObtenerPorIdAsync(usuarioId);

            if (usuarioActual == null)
            {
                return NotFound();
            }

            var usuarios = await _usuarioService.ObtenerTodosAsync(usuarioActual);
            return View(usuarios);
        }

[HttpGet]
[Authorize]
public async Task<IActionResult> EditarUsuario(int id)
{
    // Obtener el usuario actual para verificar permisos
    var usuarioActualId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    var rolActual = User.FindFirst(ClaimTypes.Role)?.Value;
    
    // Solo el administrador puede editar cualquier usuario
    // Un usuario normal solo puede editar su propio perfil
    if (rolActual != Roles.Administrador.ToString() && usuarioActualId != id)
    {
        return Forbid();
    }
    
    var usuario = await _usuarioService.ObtenerPorIdAsync(id);
    
    if (usuario == null)
    {
        return NotFound();
    }
    
    // Si el usuario tiene un cliente asociado, obtener su teléfono
    string telefono = "";
    if (usuario.Cliente != null)
    {
        telefono = usuario.Cliente.Telefono;
    }
    
    var viewModel = new ViewModel_UsuarioEditar
    {
        Id = usuario.Id,
        Nombre = usuario.Nombre,
        Apellido = usuario.Apellido,
        Telefono = telefono,
        Correo = usuario.Correo,
        Rol = usuario.Rol
    };
    
    return View(viewModel);
}

[HttpPost]
[Authorize]
[ValidateAntiForgeryToken]
public async Task<IActionResult> EditarUsuario([Bind("Id,Nombre,Apellido,Telefono,Correo,Rol")] ViewModel_UsuarioEditar viewModel)
{
    // Obtener el usuario actual para verificar permisos
    var usuarioActualId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    var rolActual = User.FindFirst(ClaimTypes.Role)?.Value;
    
    // Solo el administrador puede editar cualquier usuario
    // Un usuario normal solo puede editar su propio perfil
    if (rolActual != Roles.Administrador.ToString() && usuarioActualId != viewModel.Id)
    {
        return Forbid();
    }
    
    if (!ModelState.IsValid)
    {
        return View(viewModel);
    }
    
    try
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(viewModel.Id);
        
        if (usuario == null)
        {
            return NotFound();
        }
        
        // Verificar si el correo ya está en uso por otro usuario (solo si cambió el correo)
        if (usuario.Correo != viewModel.Correo)
        {
            var usuarioConMismoCorreo = await _usuarioService.BuscarPorCorreo(viewModel.Correo);
            if (usuarioConMismoCorreo != null && usuarioConMismoCorreo.Id != viewModel.Id)
            {
                ModelState.AddModelError("Correo", "El correo ya está siendo utilizado por otro usuario.");
                return View(viewModel);
            }
        }
        
        // Solo el administrador puede cambiar el rol
        if (rolActual != Roles.Administrador.ToString())
        {
            viewModel.Rol = usuario.Rol; // Mantener el rol original
        }
        
        // Actualizar los datos del usuario
        usuario.Nombre = viewModel.Nombre;
        usuario.Apellido = viewModel.Apellido;
        usuario.Correo = viewModel.Correo;
        usuario.Rol = viewModel.Rol;
        
        // Actualizar el usuario en la base de datos
        await _usuarioService.ActualizarAsync(usuario);
        
        // Si el usuario es de tipo cliente, actualizar también la tabla Cliente
        if (usuario.Rol == Roles.Cliente)
        {
            var cliente = await _usuarioService.ObtenerClientePorIdAsync(usuario.Id);
            if (cliente != null)
            {
                cliente.NombreCompleto = $"{viewModel.Nombre} {viewModel.Apellido}";
                cliente.Telefono = viewModel.Telefono;
                cliente.Correo = viewModel.Correo;
                
                // Ya que no hay un método específico en el servicio, actualizamos directamente
                // Nota: Sería recomendable agregar un método ActualizarClienteAsync en el servicio
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
            }
        }
        
        TempData["SuccessMessage"] = "Usuario actualizado correctamente.";
        
        // Redireccionar según el rol
        if (rolActual == Roles.Administrador.ToString())
        {
            return RedirectToAction(nameof(ListarUsuarios));
        }
        else
        {
            return RedirectToAction(nameof(PerfilUsuario), new { id = usuario.Id });
        }
    }
    catch (Exception ex)
    {
        // Loguear el error
        ModelState.AddModelError(string.Empty, "Ha ocurrido un error al actualizar el usuario.");
        return View(viewModel);
    }
    }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            // Verificar que el usuario a eliminar existe
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Verificar que no se está intentando eliminar al propio usuario administrador
            var usuarioActualId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (usuarioActualId == id)
            {
                TempData["ErrorMessage"] = "No puedes eliminar tu propia cuenta de administrador.";
                return RedirectToAction(nameof(ListarUsuarios));
            }

            return View(usuario);
        }

        [HttpPost, ActionName("EliminarUsuario")]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminarUsuario(int id)
        {
            try
            {
                // Verificar que el usuario a eliminar existe
                var usuario = await _usuarioService.ObtenerPorIdAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Verificar que no se está intentando eliminar al propio usuario administrador
                var usuarioActualId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if (usuarioActualId == id)
                {
                    TempData["ErrorMessage"] = "No puedes eliminar tu propia cuenta de administrador.";
                    return RedirectToAction(nameof(ListarUsuarios));
                }

                // Si el usuario es cliente, también eliminar sus datos de la tabla Cliente
                if (usuario.Rol == Roles.Cliente)
                {
                    var cliente = await _usuarioService.ObtenerClientePorIdAsync(id);
                    if (cliente != null)
                    {
                        _context.Clientes.Remove(cliente);
                    }
                }

                // Eliminar el usuario (asumiendo que el servicio tiene este método)
                await _usuarioService.EliminarAsync(id);

                TempData["SuccessMessage"] = "Usuario eliminado correctamente.";
                return RedirectToAction(nameof(ListarUsuarios));
            }
            catch (Exception ex)
            {
                // Loguear el error
                TempData["ErrorMessage"] = "Ha ocurrido un error al eliminar el usuario.";
                return RedirectToAction(nameof(ListarUsuarios));
            }
        }

        #endregion
    }

}
