using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class ServicioService : IServicioService
    {
        private readonly ProyectDBContext _context;

        public ServicioService(ProyectDBContext context)
        {
            _context = context;
        }

        // Obtener todos los servicios activos
        public async Task<List<Servicio>> ObtenerTodosActivosAsync()
        {
            return await _context.Servicios
                .Where(s => s.Activo)
                .Include(s => s.CitasServicios)
                .ThenInclude(cs => cs.Cita)
                .ToListAsync();
        }

        // Obtener todos los servicios (admin)
        public async Task<List<Servicio>> ObtenerTodosAsync()
        {
            return await _context.Servicios
                .Include(s => s.CitasServicios)
                .ThenInclude(cs => cs.Cita)
                .ToListAsync();
        }

        // Obtener un servicio por su ID
        public async Task<Servicio?> ObtenerPorIdAsync(int id)
        {
            return await _context.Servicios
                .Include(s => s.CitasServicios)
                .ThenInclude(cs => cs.Cita)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // Crear un nuevo servicio
        public async Task<Servicio> CrearAsync(Servicio servicio)
        {
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            return servicio;
        }

        // Actualizar un servicio existente
        public async Task<bool> ActualizarAsync(Servicio servicio)
        {
            var existente = await _context.Servicios.FindAsync(servicio.Id);

            if (existente == null)
                return false;

            existente.Nombre = servicio.Nombre;
            existente.Descripcion = servicio.Descripcion;
            existente.Precio = servicio.Precio;
            existente.Duracion = servicio.Duracion;
            existente.Activo = servicio.Activo;

            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar un servicio (lógico: marcar como inactivo)
        public async Task<bool> EliminarAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null)
                return false;

            servicio.Activo = false; // Eliminar lógicamente
            await _context.SaveChangesAsync();
            return true;
        }

        // Reactivar un servicio
        public async Task<bool> ReactivarAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);

            if (servicio == null)
                return false;

            servicio.Activo = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
