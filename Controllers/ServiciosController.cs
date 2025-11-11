using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class ServiciosController : ControllerBase
 {
 private readonly ApplicationDbContext _db;
 public ServiciosController(ApplicationDbContext db) => _db = db;

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Servicio>>> Get() => await _db.Servicios.AsNoTracking().ToListAsync();

 [HttpGet("{id:int}")]
 public async Task<ActionResult<Servicio>> Get(int id)
 {
 var s = await _db.Servicios.FindAsync(id);
 if (s == null) return NotFound();
 return s;
 }

 [HttpPost]
 public async Task<ActionResult<Servicio>> Post(Servicio servicio)
 {
 _db.Servicios.Add(servicio);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = servicio.Id }, servicio);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Put(int id, Servicio servicio)
 {
 if (id != servicio.Id) return BadRequest();
 _db.Entry(servicio).State = EntityState.Modified;
 await _db.SaveChangesAsync();
 return NoContent();
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var s = await _db.Servicios.FindAsync(id);
 if (s == null) return NotFound();
 _db.Servicios.Remove(s);
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}
