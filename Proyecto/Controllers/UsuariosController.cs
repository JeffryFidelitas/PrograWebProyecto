using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CoreLibrary.Auth;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Services.Interfaces;

namespace Proyecto.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #region Autenticación

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Correo,Contrasenna,Recordarme")] ViewModel_UsuarioLogin usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            var usuarioExistente = await _usuarioService.ObtenerPorCredencialesAsync(usuario.Correo, usuario.Contrasenna);
            if (usuarioExistente == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(usuario);
            }

            await AutenticarUsuario(usuarioExistente, usuario.Recordarme);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Nombre,Apellido,Telefono,Correo,Contrasenna")] ViewModel_UsuarioRegister usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            if (await _usuarioService.BuscarPorCorreo(usuario.Correo) != null)
            {
                ModelState.AddModelError(string.Empty, "El usuario ya existe.");
                return View(usuario);
            }

            await _usuarioService.CrearAsync(usuario);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Perfil y Gestión de Usuario

        [Authorize]
        [HttpPost] // Funcional
        public async Task<IActionResult> CambiarContrasena([FromForm] ViewModel_CambiarContrasena viewModel)
        {
            var usuarioId = RoleHelper.ObtenerId(User);
            if (usuarioId == null) return Json(new { success = false, message = "Usuario no encontrado." });

            var usuario = await _usuarioService.ObtenerPorIdAsync(usuarioId.Value);
            if (usuario == null) return Json(new { success = false, message = "Usuario no existe." });

            if (viewModel.NuevaContrasena != viewModel.ConfirmarContrasena)
                return Json(new { success = false, message = "La nueva contraseña y su confirmación no coinciden." });

            if (!await _usuarioService.VerificarContrasennaAsync(usuario, viewModel.ContrasenaActual))
                return Json(new { success = false, message = "La contraseña actual es incorrecta." });

            await _usuarioService.CambiarContrasennaAsync(usuario, viewModel.NuevaContrasena);
            return Json(new { success = true, message = "Contraseña cambiada correctamente." });
        }

        [Authorize]
        [HttpPost] // Funcional
        public async Task<IActionResult> ActualizarInformacion([FromForm] ViewModel_UsuarioPerfil viewModel)
        {
            var usuarioId = RoleHelper.ObtenerId(User);

            // Validar identidad del usuario
            if (usuarioId == null || usuarioId != viewModel.Id)
            {
                return Json(new { success = false, message = "Acceso denegado o usuario no válido." });
            }

            // Obtener usuario
            var usuario = await _usuarioService.ObtenerPorIdAsync(usuarioId.Value);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            // Verificar si el correo cambió
            if (!string.Equals(usuario.Correo, viewModel.Correo, StringComparison.OrdinalIgnoreCase))
            {
                var usuarioExistente = await _usuarioService.BuscarPorCorreo(viewModel.Correo);
                if (usuarioExistente != null)
                {
                    return Json(new { success = false, message = "El correo electrónico ya está en uso." });
                }

                // Si no existe, actualizar el correo
                usuario.Correo = viewModel.Correo;
                usuario.Cliente.Correo = viewModel.Correo;
            }

            // Actualizar nombre y apellido
            usuario.Nombre = viewModel.Nombre;
            usuario.Apellido = viewModel.Apellido;

            // Actualizar datos del cliente (si aplica)
            if (usuario.Cliente != null)
            {
                usuario.Cliente.NombreCompleto = $"{viewModel.Nombre} {viewModel.Apellido}";
                usuario.Cliente.Telefono = viewModel.Telefono;
            }

            await _usuarioService.ActualizarAsync(usuario);

            return Json(new { success = true, message = "Información actualizada correctamente." });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PerfilUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPerfilPorIdAsync(id);

            if (usuario == null) return NotFound();
            
            var usuarioActualId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (usuarioActualId != id.ToString()) return Forbid();

            return View(usuario);
        }
        #endregion

        #region Métodos Privados

        private async Task AutenticarUsuario(Usuario usuario, bool recordar)
        {
            var claims = ObtenerClaims(usuario);
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = recordar
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private List<Claim> ObtenerClaims(Usuario usuario)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Surname, usuario.Apellido),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };
        }

        #endregion
    }
}
