using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class VehiculosController : ControllerBase
 {
 private readonly ApplicationDbContext _db;

 public VehiculosController(ApplicationDbContext db)
 {
 _db = db;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Vehiculo>>> Get([FromQuery] string? placa = null, [FromQuery] int? clienteId = null, [FromQuery] int page =1, [FromQuery] int pageSize =20)
 {
 var query = _db.Vehiculos.Include(v => v.Cliente).AsNoTracking().AsQueryable();
 if (!string.IsNullOrWhiteSpace(placa))
 {
 query = query.Where(v => v.Placa.Contains(placa));
 }
 if (clienteId.HasValue)
 {
 query = query.Where(v => v.ClienteId == clienteId.Value);
 }
 page = Math.Max(1, page);
 pageSize = Math.Clamp(pageSize,1,100);
 var items = await query.Skip((page -1) * pageSize).Take(pageSize).ToListAsync();
 return items;
 }

 [HttpGet("{id:int}")]
 public async Task<ActionResult<Vehiculo>> Get(int id)
 {
 var veh = await _db.Vehiculos.Include(v => v.Cliente).AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
 if (veh == null) return NotFound();
 return veh;
 }

 [HttpGet("by-placa/{placa}")]
 public async Task<ActionResult<Vehiculo>> GetByPlaca(string placa)
 {
 var veh = await _db.Vehiculos.Include(v => v.Cliente).AsNoTracking().FirstOrDefaultAsync(v => v.Placa == placa);
 if (veh == null) return NotFound();
 return veh;
 }

 [HttpPost]
 public async Task<ActionResult<Vehiculo>> Post([FromBody] Vehiculo vehiculo)
 {
 _db.Vehiculos.Add(vehiculo);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = vehiculo.Id }, vehiculo);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Put(int id, [FromBody] Vehiculo vehiculo)
 {
 if (id != vehiculo.Id) return BadRequest();
 _db.Entry(vehiculo).State = EntityState.Modified;
 try
 {
 await _db.SaveChangesAsync();
 }
 catch (DbUpdateConcurrencyException)
 {
 if (!await _db.Vehiculos.AnyAsync(v => v.Id == id)) return NotFound();
 throw;
 }
 return NoContent();
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var veh = await _db.Vehiculos.FindAsync(id);
 if (veh == null) return NotFound();
 _db.Vehiculos.Remove(veh);
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}
