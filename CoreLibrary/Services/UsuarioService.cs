using CoreLibrary.Auth;
using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ProyectDBContext _context;

        public UsuarioService(ProyectDBContext context)
        {
            _context = context;
        }

        #region Métodos de Búsqueda

        public async Task<Usuario?> BuscarPorCorreo(string correo)
        {
            return await _context.Usuarios
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int? id)
        {
            if(id == null) return null;
            
            return await _context.Usuarios
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int usuarioId)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<ViewModel_UsuarioPerfil?> ObtenerPerfilPorIdAsync(int usuarioId)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

            if (cliente is null) return null;

            var vehiculos = await _context.Vehiculos
                .Where(v => v.ClienteId == cliente.Id)
                .ToListAsync();

            return new ViewModel_UsuarioPerfil
            {
                Nombre = cliente.Usuario.Nombre,
                Apellido = cliente.Usuario.Apellido,
                NombreCompleto = cliente.NombreCompleto,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Vehiculos = vehiculos
            };
        }

        public async Task<Usuario?> ObtenerPorCredencialesAsync(string correo, string contrasenna)
        {
            return await _context.Usuarios
                .Include(u => u.Cliente)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasenna == contrasenna);
        }

        #endregion

        #region Métodos de Validación

        public async Task<bool> ValidarCredencialesAsync(string correo, string contrasenna)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Correo == correo && u.Contrasenna == contrasenna);
        }

        #endregion

        #region Métodos de Gestión

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync(Usuario usuario)
        {
            return usuario.Rol switch
            {
                Roles.Administrador => await _context.Usuarios.Include(u => u.Cliente).ToListAsync(),
                Roles.Empleado => await _context.Usuarios
                    .Include(u => u.Cliente)
                    .Where(u => u.Rol == Roles.Cliente)
                    .ToListAsync(),
                _ => Enumerable.Empty<Usuario>()
            };
        }

        public async Task<bool> CrearAsync(ViewModel_UsuarioRegister usuario)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new Usuario
                {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Correo = usuario.Correo,
                    Contrasenna = usuario.Contrasenna,
                    Rol = Roles.Cliente,
                    FechaRegistro = DateTime.Now,
                };

                _context.Usuarios.Add(user);
                await _context.SaveChangesAsync();

                var cliente = new Cliente
                {
                    NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                    Telefono = usuario.Telefono,
                    Correo = usuario.Correo,
                    UsuarioId = user.Id
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> VerificarContrasennaAsync(Usuario usuario, string contrasenna)
        {
            return usuario.Contrasenna == contrasenna;
        }

        public async Task<bool> CambiarContrasennaAsync(Usuario usuario, string nuevaContrasenna)
        {
            if (usuario.Contrasenna == nuevaContrasenna) return false;
            usuario.Contrasenna = nuevaContrasenna;
            await _context.SaveChangesAsync();
            return true;
        }

        // Actualiza el usuario sin cambiar la contraseña
        public async Task<bool> ActualizarAsync(Usuario usuario)
        {
            var usuarioExistente = await ObtenerPorIdAsync(usuario.Id);
            if (usuarioExistente is null) return false;

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.Apellido = usuario.Apellido;
            usuarioExistente.Correo = usuario.Correo;

            // Asegurarse de que el cliente esté cargado y actualizar su teléfono
            if (usuarioExistente.Cliente != null)
            {
                usuarioExistente.Cliente.Telefono = usuario.Cliente.Telefono;
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var usuario = await ObtenerPorIdAsync(id);
            if (usuario is null) return false;

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion
    }
}
