using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class OrdenesController : ControllerBase
 {
 private readonly ApplicationDbContext _db;

 public OrdenesController(ApplicationDbContext db)
 {
 _db = db;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<OrdenServicio>>> Get()
 {
 return await _db.OrdenesServicio
 .Include(o => o.Vehiculo).Include(o => o.Empleado).Include(o => o.Detalles).ThenInclude(d => d.Servicio)
 .AsNoTracking().ToListAsync();
 }

 [HttpGet("{id:int}")]
 public async Task<ActionResult<OrdenServicio>> Get(int id)
 {
 var ord = await _db.OrdenesServicio
 .Include(o => o.Vehiculo).Include(o => o.Empleado).Include(o => o.Detalles).ThenInclude(d => d.Servicio)
 .AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);
 if (ord == null) return NotFound();
 return ord;
 }

 [HttpPost]
 public async Task<ActionResult<OrdenServicio>> Post([FromBody] OrdenServicio orden)
 {
 _db.OrdenesServicio.Add(orden);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = orden.Id }, orden);
 }

 [HttpPost("{id:int}/detalles")]
 public async Task<ActionResult> AddDetalle(int id, [FromBody] DetalleOrden detalle)
 {
 var orden = await _db.OrdenesServicio.FindAsync(id);
 if (orden == null) return NotFound();
 detalle.OrdenServicioId = id;
 _db.DetallesOrden.Add(detalle);
 await _db.SaveChangesAsync();
 return NoContent();
 }
 }
}
