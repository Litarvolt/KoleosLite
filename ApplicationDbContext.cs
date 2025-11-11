using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo
{
 public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
 {
 public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
 : base(options)
 {
 }

 // ===== ENTIDADES =====
 public DbSet<Cliente> Clientes { get; set; }
 public DbSet<Vehiculo> Vehiculos { get; set; }
 public DbSet<Empleado> Empleados { get; set; }
 public DbSet<Servicio> Servicios { get; set; }
 public DbSet<Repuesto> Repuestos { get; set; }
 public DbSet<OrdenServicio> OrdenesServicio { get; set; }
 public DbSet<DetalleOrden> DetallesOrden { get; set; }
 public DbSet<Factura> Facturas { get; set; }
 public DbSet<Usuario> Usuarios { get; set; }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
 base.OnModelCreating(modelBuilder);

 // ===============================
 // CONFIGURACIONES GENERALES
 // ===============================

 // ===== Cliente =====
 modelBuilder.Entity<Cliente>(entity =>
 {
 entity.Property(c => c.NombreCompleto)
 .IsRequired()
 .HasMaxLength(100);

 entity.Property(c => c.Correo)
 .HasMaxLength(100);

 entity.Property(c => c.Telefono)
 .HasMaxLength(20);

 entity.Property(c => c.Direccion)
 .HasMaxLength(150);
 });

 // ===== Vehículo =====
 modelBuilder.Entity<Vehiculo>(entity =>
 {
 entity.Property(v => v.Placa)
 .IsRequired()
 .HasMaxLength(15);

 entity.Property(v => v.Marca)
 .IsRequired()
 .HasMaxLength(50);

 entity.Property(v => v.Modelo)
 .HasMaxLength(50);

 entity.Property(v => v.Color)
 .HasMaxLength(30);

 entity.Property(v => v.Vin)
 .HasMaxLength(50);
 });

 // ===== Empleado =====
 modelBuilder.Entity<Empleado>(entity =>
 {
 entity.Property(e => e.NombreCompleto)
 .IsRequired()
 .HasMaxLength(100);

 entity.Property(e => e.Cargo)
 .HasMaxLength(50);

 entity.Property(e => e.Correo)
 .HasMaxLength(100);

 entity.Property(e => e.Telefono)
 .HasMaxLength(20);

 entity.Property(e => e.Salario)
 .HasColumnType("decimal(10,2)");
 });

 // ===== Servicio =====
 modelBuilder.Entity<Servicio>(entity =>
 {
 entity.Property(s => s.Nombre)
 .IsRequired()
 .HasMaxLength(100);

 entity.Property(s => s.Descripcion)
 .HasMaxLength(250);

 entity.Property(s => s.CostoBase)
 .HasColumnType("decimal(10,2)");
 });

 // ===== Repuesto =====
 modelBuilder.Entity<Repuesto>(entity =>
 {
 entity.Property(r => r.Nombre)
 .IsRequired()
 .HasMaxLength(100);

 entity.Property(r => r.Codigo)
 .HasMaxLength(50);

 entity.Property(r => r.PrecioUnitario)
 .HasColumnType("decimal(10,2)");

 entity.Property(r => r.Stock)
 .HasDefaultValue(0);
 });

 // ===== OrdenServicio =====
 modelBuilder.Entity<OrdenServicio>(entity =>
 {
 entity.Property(o => o.FechaEntrada)
 .IsRequired();

 entity.Property(o => o.FechaSalida);

 entity.Property(o => o.Estado)
 .HasMaxLength(50);

 entity.Property(o => o.Observaciones)
 .HasMaxLength(500);
 });

 // ===== DetalleOrden =====
 modelBuilder.Entity<DetalleOrden>(entity =>
 {
 entity.Property(d => d.Cantidad)
 .HasDefaultValue(1);

 entity.Property(d => d.PrecioUnitario)
 .HasColumnType("decimal(10,2)");
 });

 // ===== Factura =====
 modelBuilder.Entity<Factura>(entity =>
 {
 entity.Property(f => f.FechaEmision)
 .IsRequired();

 entity.Property(f => f.Total)
 .HasColumnType("decimal(10,2)");
 });

 // ===== Usuario =====
 modelBuilder.Entity<Usuario>(entity =>
 {
 entity.Property(u => u.NombreUsuario)
 .IsRequired()
 .HasMaxLength(50);

 entity.Property(u => u.Correo)
 .HasMaxLength(100);

 entity.Property(u => u.ContrasenaHash)
 .HasMaxLength(200);

 entity.Property(u => u.Rol)
 .HasMaxLength(30);
 });

 // ===============================
 // RELACIONES Y RESTRICCIONES
 // ===============================

 // Cliente1:N Vehiculos
 modelBuilder.Entity<Cliente>()
 .HasMany(c => c.Vehiculos)
 .WithOne(v => v.Cliente)
 .HasForeignKey(v => v.ClienteId)
 .OnDelete(DeleteBehavior.Cascade);

 // Vehiculo1:N OrdenesServicio
 modelBuilder.Entity<Vehiculo>()
 .HasMany(v => v.Ordenes)
 .WithOne(o => o.Vehiculo)
 .HasForeignKey(o => o.VehiculoId)
 .OnDelete(DeleteBehavior.Cascade);

 // Empleado1:N OrdenesServicio
 modelBuilder.Entity<Empleado>()
 .HasMany(e => e.OrdenesAsignadas)
 .WithOne(o => o.Empleado)
 .HasForeignKey(o => o.EmpleadoId)
 .OnDelete(DeleteBehavior.Restrict);

 // OrdenServicio1:N Detalles
 modelBuilder.Entity<OrdenServicio>()
 .HasMany(o => o.Detalles)
 .WithOne(d => d.OrdenServicio)
 .HasForeignKey(d => d.OrdenServicioId)
 .OnDelete(DeleteBehavior.Cascade);

 // OrdenServicio1:1 Factura - CAMBIO IMPORTANTE
 modelBuilder.Entity<OrdenServicio>()
 .HasOne(o => o.Factura)
 .WithOne(f => f.OrdenServicio)
 .HasForeignKey<Factura>(f => f.OrdenServicioId)
 .OnDelete(DeleteBehavior.Cascade);

 // Factura N:1 Cliente - CAMBIO IMPORTANTE
 modelBuilder.Entity<Factura>()
 .HasOne(f => f.Cliente)
 .WithMany() // No hay colección de Facturas en Cliente
 .HasForeignKey(f => f.ClienteId)
 .OnDelete(DeleteBehavior.Restrict); // Cambiar a Restrict

 // Servicio1:N DetalleOrden
 modelBuilder.Entity<DetalleOrden>()
 .HasOne(d => d.Servicio)
 .WithMany(s => s.DetallesOrden)
 .HasForeignKey(d => d.ServicioId)
 .OnDelete(DeleteBehavior.Restrict);

 // Repuesto1:N DetalleOrden
 modelBuilder.Entity<DetalleOrden>()
 .HasOne(d => d.Repuesto)
 .WithMany(r => r.DetallesOrden)
 .HasForeignKey(d => d.RepuestoId)
 .OnDelete(DeleteBehavior.Restrict);
 }
 }
}