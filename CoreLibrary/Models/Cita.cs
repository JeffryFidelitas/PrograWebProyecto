using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    public class Cita
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int VehiculoId { get; set; }

        [Required(ErrorMessage = "Fecha y hora es requerida")]
        [Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; }

        [EnumDataType(typeof(EstadoCita))]
        public EstadoCita Estado { get; set; }

        public Cliente Cliente { get; set; }

        public Vehiculo Vehiculo { get; set; }

        public ICollection<CitaServicio> CitasServicios { get; set; }
    }

    public class CitaServicio
    {
        public int CitaId { get; set; }
        public int ServicioId { get; set; }

        public Cita Cita { get; set; }
        public Servicio Servicio { get; set; }
    }

    public enum EstadoCita
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Completada
    }
}