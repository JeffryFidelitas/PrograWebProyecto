using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models.ViewModels
{
    public class ViewModel_UsuarioPerfil
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El completo no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El apellido solo puede contener letras y espacios.")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        [Display(Name = "Nombre")]
        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(100, ErrorMessage = "El correo electrónico no puede exceder los 100 caracteres.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "El correo electrónico no es válido.")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; }

        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "El número de teléfono no es válido.")]
        [MaxLength(15)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        public List<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }

    public class ViewModel_CambiarContrasena
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string ContrasenaActual { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$",
            ErrorMessage = "La contraseña debe contener al menos una letra mayúscula, una letra minúscula y un número.")]
        [Display(Name = "Nueva contraseña")]
        public string NuevaContrasena { get; set; }

        [Required(ErrorMessage = "Debe confirmar la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        [Display(Name = "Confirmar nueva contraseña")]
        public string ConfirmarContrasena { get; set; }
    }

    public class ViewModel_Vehiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La placa es requerida.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "La marca es requerida.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "El modelo es requerido.")]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "El color es requerido.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "El tipo es requerido.")]
        public string Tipo { get; set; }
    }
}
