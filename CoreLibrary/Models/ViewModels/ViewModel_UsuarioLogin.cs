using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models.ViewModels
{
    public class ViewModel_UsuarioLogin
    {
        [Required(ErrorMessage = "Correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo no es válido")]
        [MaxLength(100)]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "Contraseña es requerida")]
        [Display(Name = "Contraseña")]
        public string Contrasenna { get; set; }
    }
}
