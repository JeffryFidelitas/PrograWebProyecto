using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Cita
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tipo de auto es un campo requerido"), Display(Name = "Tipo de Auto")]
        public TiposAuto TipoAuto { get; set; }

        [Required(ErrorMessage = "Fecha es un campo requerido"), Display(Name = "Fecha")]
        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "Hora es un campo requerido"), Display(Name = "Hora")]
        public TimeOnly Hora { get; set; }

        [Display(Name = "Tipo de Lavado")]
        public Lavado? TipoLavado { get; set; }

        [Display(Name = "Realizada")]
        public bool Realizada { get; set; } = false;

        [Required]
        public Usuario? usuario { get; set; }
    }

    public enum TiposAuto
    {
        SUV,
        Compacto,
        Buseta,
        Pickup,
        Urbano
    }
}