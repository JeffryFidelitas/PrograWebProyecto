using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models
{
    public class Cliente
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Nombre completo es requerido")]
        [MaxLength(100)]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "El número de teléfono no es válido.")]
        [MaxLength(15)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Correo { get; set; }

        public Usuario Usuario { get; set; }

        public ICollection<Vehiculo> Vehiculos { get; set; }

        public ICollection<Cita> Citas { get; set; }

        public ICollection<Notificacion> Notificaciones { get; set; }
    }
}
