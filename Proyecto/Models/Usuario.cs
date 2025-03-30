using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nombre es un campo requerido"), Display(Name = "Nombre")]
        public string? Nombre { get; set; }
        
        [Required(ErrorMessage = "Apellidos es un campo requerido"), Display(Name = "Apellidos")]
        public string? Apellidos { get; set; }

        [Required(ErrorMessage = "Correo Electrónico es un campo requerido"), Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Contraseña es un campo requerido"), Display(Name = "Contraseña")]
        public string? Clave { get; set; }

        [Required(ErrorMessage = "Rol es un campo requerido"), Display(Name = "Rol")]
        public Roles Rol { get; set; }

        public virtual ImmutableArray<Cita> citas { get; set; }
    }
    public enum Roles {
        Administrador,
        Cliente
    }
}