namespace KoleosDemo.Entidades
{
    public class OrdenServicio
    {
        public int Id { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime? FechaSalida { get; set; }
        public string Estado { get; set; } = "Pendiente"; // Pendiente, En Proceso, Finalizado, Cancelado
        public string Observaciones { get; set; } = null!;

        // Relaciones
        public int VehiculoId { get; set; }
        public Vehiculo Vehiculo { get; set; } = null!;

        public int EmpleadoId { get; set; }
        public Empleado Empleado { get; set; } = null!;

        public ICollection<DetalleOrden> Detalles { get; set; } = new List<DetalleOrden>();
        public Factura? Factura { get; set; }
    }
}