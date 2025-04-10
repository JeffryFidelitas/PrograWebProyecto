using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using CoreLibrary.Models.ViewModels;

namespace Proyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
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
        #endregion
    }
}
