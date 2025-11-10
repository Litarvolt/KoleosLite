namespace KoleosDemo.Entidades
{
    public class Cliente
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Direccion { get; set; } = null!;

        // Relaciones
        public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
    }

}