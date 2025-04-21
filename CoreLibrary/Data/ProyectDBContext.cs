using Microsoft.EntityFrameworkCore;
using CoreLibrary.Models;
using CoreLibrary.Auth;

namespace CoreLibrary.Data
{
    public class ProyectDBContext : DbContext
    {
        public ProyectDBContext(DbContextOptions<ProyectDBContext> options) : base(options) { }

        #region DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<CitaServicio> CitasServicios { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        #endregion

        #region Overrides
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Propiedad de precisión para precios
            modelBuilder.Entity<Servicio>()
                .Property(s => s.Precio)
                .HasPrecision(10, 2);

            // Usuario - Cliente (uno a uno)
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Cliente)
                .WithOne(c => c.Usuario)
                .HasForeignKey<Cliente>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente - Vehículos (uno a muchos)
            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.Cliente)
                .WithMany(c => c.Vehiculos)
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente - Citas (uno a muchos)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Cliente)
                .WithMany(c => c.Citas)
                .HasForeignKey(c => c.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);

            // Vehículo - Citas (uno a muchos)
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Vehiculo)
                .WithMany(v => v.Citas)
                .HasForeignKey(c => c.VehiculoId)
                .OnDelete(DeleteBehavior.NoAction);

            // Cita - Servicio (muchos a muchos)
            modelBuilder.Entity<CitaServicio>()
                .HasKey(cs => new { cs.CitaId, cs.ServicioId });

            // Cita - Servicio (muchos a muchos con navegación inversa)
            modelBuilder.Entity<CitaServicio>()
                .HasOne(cs => cs.Cita)
                .WithMany(c => c.CitasServicios)
                .HasForeignKey(cs => cs.CitaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Servicio - Cita (muchos a muchos)
            modelBuilder.Entity<CitaServicio>()
                .HasOne(cs => cs.Servicio)
                .WithMany(s => s.CitasServicios)
                .HasForeignKey(cs => cs.ServicioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cliente - Notificaciones (uno a muchos con navegación inversa)
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Cliente)
                .WithMany(c => c.Notificaciones)
                .HasForeignKey(n => n.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cita - Notificaciones (opcional)
            modelBuilder.Entity<Notificacion>()
                .HasOne(n => n.Cita)
                .WithMany()
                .HasForeignKey(n => n.CitaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Usuario - Reportes (uno a muchos con navegación inversa)
            modelBuilder.Entity<Reporte>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reportes)
                .HasForeignKey(r => r.GeneradoPor)
                .OnDelete(DeleteBehavior.Cascade);

            #region Índices
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

            modelBuilder.Entity<Vehiculo>()
                .HasIndex(v => v.Placa)
                .IsUnique();

            modelBuilder.Entity<Cita>()
                .HasIndex(c => c.FechaHora);
            #endregion

            #region Servicios Iniciales
            modelBuilder.Entity<Servicio>().HasData(
                new Servicio
                {
                    Id = -1,
                    Nombre = "Cambio de Aceite",
                    Descripcion = "Cambio de aceite y filtro.",
                    Precio = 29.99m,
                    Duracion = 60
                },
                new Servicio
                {
                    Id = -2,
                    Nombre = "Revisión de Frenos",
                    Descripcion = "Inspección y ajuste de frenos.",
                    Precio = 49.99m,
                    Duracion = 90
                }
            );
            #endregion

            #region Usuarios Iniciales
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = -1,
                    Nombre = "Randall",
                    Apellido = "Montero",
                    Correo = "randall@gmail.com",
                    Contrasenna = "Pass123",
                    Rol = Roles.Administrador,
                    FechaRegistro = new DateTime(2025, 4, 7, 12, 0, 0)
                },
                new Usuario
                {
                    Id = -2,
                    Nombre = "Jeffrey",
                    Apellido = "Samuel",
                    Correo = "jeffrey@gmail.com",
                    Contrasenna = "Pass123",
                    Rol = Roles.Cliente,
                    FechaRegistro = new DateTime(2025, 4, 7, 12, 0, 0)
                }
            );
            #endregion

            #region Clientes Iniciales
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    Id = -2,
                    UsuarioId = -1,
                    NombreCompleto = "Randall Montero",
                    Telefono = "12345678",
                    Correo = "randall@gmail.com"
                },
                new Cliente
                {
                    Id = -1,
                    UsuarioId = -2,
                    NombreCompleto = "Jeffrey Samuel",
                    Telefono = "12345678",
                    Correo = "jeffrey@gmail.com"
                }
            );
            #endregion

            #region Vehículos Iniciales
            modelBuilder.Entity<Vehiculo>().HasData(
                new Vehiculo
                {
                    Id = -1,
                    ClienteId = -1,
                    Placa = "ABC123",
                    Tipo = "Sedán",
                    Marca = "Toyota",
                    Modelo = "Corolla",
                    Color = "Rojo"
                },
                new Vehiculo
                {
                    Id = -2,
                    ClienteId = -1,
                    Placa = "XYZ789",
                    Tipo = "SUV",
                    Marca = "Honda",
                    Modelo = "Civic",
                    Color = "Azul"
                }
            );
            #endregion

            #region Citas Iniciales
            modelBuilder.Entity<Cita>().HasData(
                new Cita
                {
                    Id = -1,
                    ClienteId = -1,
                    VehiculoId = -1,
                    FechaHora = new DateTime(2025, 4, 10, 12, 0, 0),
                    Estado = EstadoCita.Pendiente
                },
                new Cita
                {
                    Id = -2,
                    ClienteId = -1,
                    VehiculoId = -2,
                    FechaHora = new DateTime(2025, 4, 12, 12, 0, 0),
                    Estado = EstadoCita.Confirmada
                }
            );
            #endregion

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
