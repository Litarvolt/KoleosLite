using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class EmpleadosController : ControllerBase
 {
 private readonly ApplicationDbContext _db;
 public EmpleadosController(ApplicationDbContext db) => _db = db;

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Empleado>>> Get() => await _db.Empleados.AsNoTracking().ToListAsync();

 [HttpGet("{id:int}")]
 public async Task<ActionResult<Empleado>> Get(int id)
 {
 var e = await _db.Empleados.FindAsync(id);
 if (e == null) return NotFound();
 return e;
 }

 [HttpPost]
 public async Task<ActionResult<Empleado>> Post(Empleado empleado)
 {
 _db.Empleados.Add(empleado);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = empleado.Id }, empleado);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Put(int id, Empleado empleado)
 {
 if (id != empleado.Id) return BadRequest();
 _db.Entry(empleado).State = EntityState.Modified;
 await _db.SaveChangesAsync();
 return NoContent();
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var e = await _db.Empleados.FindAsync(id);
 if (e == null) return NotFound();
 _db.Empleados.Remove(e);
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}
