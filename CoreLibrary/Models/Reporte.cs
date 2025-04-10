using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace CoreLibrary.Models
{
    public class Reporte
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime FechaGenerado { get; set; } = DateTime.Now;

        public string TipoReporte { get; set; }

        public string Datos { get; set; }

        public int GeneradoPor { get; set; }

        public Usuario Usuario { get; set; }
    }
}
