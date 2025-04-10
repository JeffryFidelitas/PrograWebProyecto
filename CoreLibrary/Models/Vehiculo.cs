using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models
{
    public class Vehiculo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "Placa es requerida")]
        [MaxLength(20)]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Tipo de vehículo es requerido")]
        public string Tipo { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Color { get; set; }

        public Cliente Cliente { get; set; }

        public ICollection<Cita> Citas { get; set; }
    }
}
