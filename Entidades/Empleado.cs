namespace KoleosDemo.Entidades
{
    public class Empleado
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = null!;
        public string Cargo { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public decimal Salario { get; set; }

        public ICollection<OrdenServicio> OrdenesAsignadas { get; set; } = new List<OrdenServicio>();
    }
}