using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreLibrary.Auth;

namespace CoreLibrary.Models
{
    public class Usuario
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellido es requerido")]
        [MaxLength(100)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo no es válido")]
        [MaxLength(100)]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        [Display(Name = "Contraseña")]
        public string Contrasenna { get; set; }

        [Required(ErrorMessage = "Rol es requerido")]
        public Roles Rol { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now; // ← Esto es válido para usuarios nuevos, pero no se usará en HasData()

        public Cliente Cliente { get; set; }

        public ICollection<Reporte> Reportes { get; set; }

    }
}