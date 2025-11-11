using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class RepuestosController : ControllerBase
 {
 private readonly ApplicationDbContext _db;
 public RepuestosController(ApplicationDbContext db) => _db = db;

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Repuesto>>> Get() => await _db.Repuestos.AsNoTracking().ToListAsync();

 [HttpGet("{id:int}")]
 public async Task<ActionResult<Repuesto>> Get(int id)
 {
 var r = await _db.Repuestos.FindAsync(id);
 if (r == null) return NotFound();
 return r;
 }

 [HttpPost]
 public async Task<ActionResult<Repuesto>> Post(Repuesto repuesto)
 {
 _db.Repuestos.Add(repuesto);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = repuesto.Id }, repuesto);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Put(int id, Repuesto repuesto)
 {
 if (id != repuesto.Id) return BadRequest();
 _db.Entry(repuesto).State = EntityState.Modified;
 await _db.SaveChangesAsync();
 return NoContent();
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var r = await _db.Repuestos.FindAsync(id);
 if (r == null) return NotFound();
 _db.Repuestos.Remove(r);
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}
