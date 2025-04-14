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

        #region Consultas

        public async Task<Usuario?> BuscarPorCorreo(string correo)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        }

        public async Task<Usuario?> BuscarPorCredenciales(string correo, string contrasenna)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasenna == contrasenna);
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync(Usuario usuario)
        {
            // Si el usuario es administrador le carga todos los usuarios y si es empleado solo los clientes
            if (usuario.Rol == Roles.Administrador)
            {
                return await _context.Usuarios
                    .Include(u => u.Cliente)
                    .ToListAsync();
            }
            else if (usuario.Rol == Roles.Empleado)
            {
                return await _context.Usuarios
                    .Include(u => u.Cliente)
                    .Where(u => u.Rol == Roles.Cliente)
                    .ToListAsync();
            }

            return Enumerable.Empty<Usuario>();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios
                         .Include(u => u.Cliente)
                         .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return await _context.Clientes
                        .FirstOrDefaultAsync(c => c.UsuarioId == id);
        }

        public async Task<ViewModel_UsuarioPerfilCompleto> ObtenerPerfilPorIdAsync(int id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.UsuarioId == id);

            if (cliente == null)
            {
                return null;
            }

            var vehiculos = await _context.Vehiculos
                .Where(v => v.ClienteId == cliente.Id)
                .ToListAsync();

            var perfilCompleto = new ViewModel_UsuarioPerfilCompleto
            {
                NombreCompleto = cliente.NombreCompleto,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                Vehiculos = vehiculos
            };

            return perfilCompleto;
        }

        #endregion

        #region Crear

        public async Task CrearAsync(ViewModel_UsuarioRegister usuario)
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
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Ocurrió un error al registrar el usuario y cliente.", ex);
            }
        }

        #endregion

        #region Actualizar

        public async Task ActualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Eliminar

        public async Task EliminarAsync(int id)
        {
            var usuario = await ObtenerPorIdAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        #endregion
    }
}
