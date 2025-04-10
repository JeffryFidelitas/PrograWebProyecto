using CoreLibrary.Data;
using CoreLibrary.Models;
using CoreLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services
{
    public class CitaService : ICitaService
    {
        private readonly ProyectDBContext _context;

        public CitaService(ProyectDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cita>> ObtenerTodasAsync(int usuarioId, bool esAdmin)
        {
            var query = _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .Include(c => c.CitasServicios)
                    .ThenInclude(cs => cs.Servicio)
                .AsQueryable();

            if (!esAdmin)
            {
                query = query.Where(c => c.Cliente.UsuarioId == usuarioId);
            }

            return await query.ToListAsync();
        }

        public async Task<Cita?> ObtenerPorIdAsync(int id)
        {
            return await _context.Citas
                .Include(c => c.Cliente)
                .Include(c => c.Vehiculo)
                .Include(c => c.CitasServicios)
                    .ThenInclude(cs => cs.Servicio)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CrearAsync(Cita cita)
        {
            if (cita is null)
                throw new ArgumentNullException(nameof(cita));

            if (cita.CitasServicios == null)
                cita.CitasServicios = new List<CitaServicio>();

            await _context.Citas.AddAsync(cita);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Cita cita)
        {
            if (cita is null)
                throw new ArgumentNullException(nameof(cita));

            var existente = await _context.Citas
                .Include(c => c.CitasServicios)
                .FirstOrDefaultAsync(c => c.Id == cita.Id);

            if (existente == null)
                throw new InvalidOperationException("La cita no existe.");

            // Actualiza los campos
            existente.FechaHora = cita.FechaHora;
            existente.Estado = cita.Estado;
            existente.VehiculoId = cita.VehiculoId;

            _context.Citas.Update(existente);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita is null)
                return;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
        }
    }
}
