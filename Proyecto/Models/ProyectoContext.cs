using Microsoft.EntityFrameworkCore;

namespace Proyecto.Models
{
    public class ProyectoContext : DbContext
    {
        public ProyectoContext(DbContextOptions<ProyectoContext> options) : base(options)
        {}

        public DbSet<Cita> Citas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Lavado> Lavado { get; set; }   

    }
}