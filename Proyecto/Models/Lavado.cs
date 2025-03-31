using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Lavado
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del lavado es requerido")]
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El precio es requerido")]
        public decimal Precio { get; set; }

        [DisplayName("Duración")]
        [Required(ErrorMessage = "La duración es requerida")]
        public int Duracion { get; set; }
        public bool Estado { get; set; } = true;
    }
}
