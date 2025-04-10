using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models.ViewModels
{
    public class ViewModel_UsuarioPerfilCompleto
    {
        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(100)]
        [Display(Name = "Nombre")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo no es válido")]
        [MaxLength(100)]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "El número de teléfono no es válido.")]
        [MaxLength(15)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        public List<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();


    }
}
