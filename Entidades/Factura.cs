namespace KoleosDemo.Entidades
{
    public class Factura
    {
        public int Id { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Total { get; set; }

        // Relaciones
        public int OrdenServicioId { get; set; }
        public OrdenServicio OrdenServicio { get; set; } = null!;

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
    }
}