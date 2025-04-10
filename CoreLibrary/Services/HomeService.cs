using CoreLibrary.Auth;
using CoreLibrary.Data;
using CoreLibrary.Models.ViewModels;
using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using CoreLibrary.Services.Interfaces;

namespace CoreLibrary.Services
{
    public class HomeService : IHomeService
    {
        private readonly ProyectDBContext _context;
        public HomeService(ProyectDBContext context)
        {
            _context = context;
        }
        public async Task<ViewModel_Index> ObtenerIndexParaInvitado()
        {
            return new ViewModel_Index
            {
                Servicios = await _context.Servicios
                    .Where(s => s.Activo)
                    .ToListAsync()
            };
        }

        public async Task<ViewModel_Index> ObtenerIndexParaUsuario(Usuario usuario)
        {
            if (usuario == null) throw new ArgumentNullException(nameof(usuario));

            var modelo = new ViewModel_Index();

            // Servicios activos (disponibles para todos los usuarios)
            modelo.Servicios = await _context.Servicios
                .Where(s => s.Activo)
                .ToListAsync();

            switch (usuario.Rol)
            {
                case Roles.Administrador:
                case Roles.Empleado:
                    // Información para Admins y Empleados
                    modelo.CitasPendientes = await _context.Citas.CountAsync(c => c.Estado == EstadoCita.Pendiente);
                    modelo.TotalClientes = await _context.Usuarios.CountAsync(u => u.Rol == Roles.Cliente);
                    modelo.TotalServicios = await _context.Servicios.CountAsync();
                    modelo.TotalReportes = await _context.Reportes.CountAsync();
                    break;

                case Roles.Cliente:
                    // Información específica para clientes
                    var cliente = await _context.Clientes
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.UsuarioId == usuario.Id);

                    if (cliente != null)
                    {
                        modelo.MisCitasPendientes = await _context.Citas.CountAsync(c => c.ClienteId == cliente.Id && c.Estado == EstadoCita.Pendiente);
                        modelo.MisCitasConfirmadas = await _context.Citas.CountAsync(c => c.ClienteId == cliente.Id && c.Estado == EstadoCita.Confirmada);
                    }
                    break;
            }

            return modelo;
        }
    }
}
