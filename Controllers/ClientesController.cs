using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class ClientesController : ControllerBase
 {
 private readonly ApplicationDbContext _db;

 public ClientesController(ApplicationDbContext db)
 {
 _db = db;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Cliente>>> Get()
 {
 return await _db.Clientes.AsNoTracking().ToListAsync();
 }

 [HttpGet("{id:int}")]
 public async Task<ActionResult<Cliente>> Get(int id)
 {
 var cliente = await _db.Clientes.Include(c => c.Vehiculos).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
 if (cliente == null) return NotFound();
 return cliente;
 }

 [HttpPost]
 public async Task<ActionResult<Cliente>> Post([FromBody] Cliente cliente)
 {
 _db.Clientes.Add(cliente);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
 }

 [HttpPut("{id:int}")]
 public async Task<IActionResult> Put(int id, [FromBody] Cliente cliente)
 {
 if (id != cliente.Id) return BadRequest();
 _db.Entry(cliente).State = EntityState.Modified;
 try
 {
 await _db.SaveChangesAsync();
 }
 catch (DbUpdateConcurrencyException)
 {
 if (!await _db.Clientes.AnyAsync(c => c.Id == id)) return NotFound();
 throw;
 }
 return NoContent();
 }

 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var cliente = await _db.Clientes.FindAsync(id);
 if (cliente == null) return NotFound();
 _db.Clientes.Remove(cliente);
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}
