using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.Models
{
    public class Notificacion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int? CitaId { get; set; }

        [Required(ErrorMessage = "Mensaje es requerido")]
        public string Mensaje { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public string Tipo { get; set; }

        public Cliente Cliente { get; set; }

        public Cita Cita { get; set; }
    }
}
