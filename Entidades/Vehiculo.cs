namespace KoleosDemo.Entidades
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Placa { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public int Anio { get; set; }
        public string Color { get; set; } = null!;
        public string Vin { get; set; } = null!; // opcional, útil para control técnico

        // Relaciones
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public ICollection<OrdenServicio> Ordenes { get; set; } = new List<OrdenServicio>();
    }
}