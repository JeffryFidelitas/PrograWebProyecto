namespace CoreLibrary.Models.ViewModels
{
    public class ViewModel_Index
    {
        // Para admin o empleado
        public int? CitasPendientes { get; set; }
        public int? TotalClientes { get; set; }
        public int? TotalServicios { get; set; }
        public int? TotalReportes { get; set; }

        // Para cliente
        public int? MisCitasConfirmadas { get; set; }
        public int? MisCitasPendientes { get; set; }

        // Si luego querés mostrar citas o listas
        public List<Cita>? ListaCitasHoy { get; set; }
        public List<Notificacion>? Notificaciones { get; set; }
        public List<Servicio>? Servicios { get; set; }
    }
}
