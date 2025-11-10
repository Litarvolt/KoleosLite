namespace KoleosDemo.Entidades
{
    public class Repuesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public decimal PrecioUnitario { get; set; }
        public int Stock { get; set; }

        // Relaciones
        public ICollection<DetalleOrden> DetallesOrden { get; set; } = new List<DetalleOrden>();
    }
}