using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    public class Servicio
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del servicio es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        [MinLength(3, ErrorMessage = "El nombre debe tener al menos 3 caracteres")]
        [DisplayName("Nombre del servicio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripción es requerida")]
        [MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
        [MinLength(10, ErrorMessage = "La descripción debe tener al menos 10 caracteres")]
        [DisplayName("Descripción del servicio")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Precio es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "Precio debe ser un valor positivo")]
        [DisplayName("Precio del servicio")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [DisplayName("Duración estimada (minutos)")]
        [Required(ErrorMessage = "Duración estimada es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "Duración estimada debe ser mayor a 0")]
        public int Duracion { get; set; }

        [DisplayName("Estado del servicio")]
        public bool Activo { get; set; } = true;

        public ICollection<CitaServicio> CitasServicios { get; set; }
    }
}
