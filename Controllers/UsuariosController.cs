using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class UsuariosController : ControllerBase
 {
 private readonly ApplicationDbContext _db;
 public UsuariosController(ApplicationDbContext db) => _db = db;

 [HttpGet]
 [Authorize(Roles = "Admin")]
 public async Task<ActionResult<IEnumerable<Usuario>>> Get()
 {
 return await _db.Usuarios.AsNoTracking().ToListAsync();
 }

 // Register a new user. Body: { nombreUsuario, correo, password, rol }
 [HttpPost("register")]
 [AllowAnonymous]
 public async Task<ActionResult<Usuario>> Register([FromBody] RegisterModel model)
 {
 if (await _db.Usuarios.AnyAsync(u => u.NombreUsuario == model.NombreUsuario))
 return BadRequest("Usuario ya existe");

 var (hash, salt) = HashPassword(model.Password);
 var stored = Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);

 var user = new Usuario
 {
 NombreUsuario = model.NombreUsuario,
 Correo = model.Correo,
 ContrasenaHash = stored,
 Rol = string.IsNullOrWhiteSpace(model.Rol) ? "User" : model.Rol
 };

 _db.Usuarios.Add(user);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
 }

 // Authenticate: returns { ok=true, rol="Admin" } on success
 [HttpPost("authenticate")]
 [AllowAnonymous]
 public async Task<ActionResult<AuthResult>> Authenticate([FromBody] LoginModel model)
 {
 var user = await _db.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario);
 if (user == null) return Unauthorized();

 var parts = (user.ContrasenaHash ?? string.Empty).Split(':');
 if (parts.Length !=2) return Unauthorized();
 var salt = Convert.FromBase64String(parts[0]);
 var storedHash = Convert.FromBase64String(parts[1]);

 if (VerifyPassword(model.Password, salt, storedHash))
 {
 return new AuthResult { Ok = true, Rol = user.Rol, NombreUsuario = user.NombreUsuario };
 }

 return Unauthorized();
 }

 // Helpers
 private static (byte[] hash, byte[] salt) HashPassword(string password)
 {
 using var rng = RandomNumberGenerator.Create();
 var salt = new byte[16];
 rng.GetBytes(salt);
 using var pbkdf2 = new Rfc2898DeriveBytes(password, salt,100_000, HashAlgorithmName.SHA256);
 var hash = pbkdf2.GetBytes(32);
 return (hash, salt);
 }

 private static bool VerifyPassword(string password, byte[] salt, byte[] expectedHash)
 {
 using var pbkdf2 = new Rfc2898DeriveBytes(password, salt,100_000, HashAlgorithmName.SHA256);
 var hash = pbkdf2.GetBytes(32);
 return CryptographicOperations.FixedTimeEquals(hash, expectedHash);
 }

 public record RegisterModel(string NombreUsuario, string Correo, string Password, string? Rol);
 public record LoginModel(string NombreUsuario, string Password);
 public record AuthResult { public bool Ok { get; init; } public string? Rol { get; init; } public string? NombreUsuario { get; init; } }
 }
}
