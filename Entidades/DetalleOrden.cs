namespace KoleosDemo.Entidades
{
    public class DetalleOrden
    {
        public int Id { get; set; }

        public int OrdenServicioId { get; set; }
        public OrdenServicio OrdenServicio { get; set; } = null!;

        public int? ServicioId { get; set; }
        public Servicio? Servicio { get; set; }

        public int? RepuestoId { get; set; }
        public Repuesto? Repuesto { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}