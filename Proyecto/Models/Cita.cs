using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tipo de auto es un campo requerido"), Display(Name = "Tipo de Auto")]
        public tiposAuto TipoAuto { get; set; }

        [Required(ErrorMessage = "Fecha y hora es un campo requerido"), Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; }

        [Display(Name = "Precio")]
        public float Precio { get; set; }

        [Display(Name = "Realizada")]
        public bool Realizada { get; set; } = false;

        [Required]
        public Usuario? usuario { get; set; }
    }

    public enum tiposAuto
    {
        SUV,
        Compacto,
        Buseta,
        Pickup,
        Urbano
    }
}