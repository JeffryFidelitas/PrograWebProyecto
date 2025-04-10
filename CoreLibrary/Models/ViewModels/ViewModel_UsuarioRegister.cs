using System.ComponentModel.DataAnnotations;
using CoreLibrary.Auth;

namespace CoreLibrary.Models.ViewModels
{
    public class ViewModel_UsuarioRegister
    {
        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "Apellido es requerido")]
        [MaxLength(100)]
        [Display(Name = "Apellido")]
        public required string Apellido { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "El número de teléfono no es válido.")]
        [MaxLength(15)]
        [Display(Name = "Teléfono")]
        public required string Telefono { get; set; }

        [Required(ErrorMessage = "Correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo no es válido")]
        [MaxLength(100)]
        [Display(Name = "Correo Electrónico")]
        public required string Correo { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        [Display(Name = "Contraseña")]
        public required string Contrasenna { get; set; }

        [Required(ErrorMessage = "Rol es requerido")]
        public Roles Rol { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now; // ← Esto es válido para usuarios nuevos, pero no se usará en HasData()

        public Cliente? Cliente { get; set; } // Esto es opcional, ya que el cliente se puede crear en otro lugar
    }
}
