namespace KoleosDemo.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string ContrasenaHash { get; set; } = null!;
        public string Rol { get; set; } = "Administrador"; // o Mecanico, Cajero, etc.
    }
}