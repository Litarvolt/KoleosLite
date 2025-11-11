using KoleosDemo;
using KoleosDemo.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoleosDemo.Controllers
{
 [ApiController]
 [Route("api/[controller]")]
 public class FacturasController : ControllerBase
 {
 private readonly ApplicationDbContext _db;
 public FacturasController(ApplicationDbContext db) => _db = db;

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Factura>>> Get() => await _db.Facturas.Include(f => f.Cliente).AsNoTracking().ToListAsync();

 [HttpGet("{id:int}")]
 public async Task<ActionResult<Factura>> Get(int id)
 {
 var f = await _db.Facturas.Include(fa => fa.Cliente).Include(fa => fa.OrdenServicio).ThenInclude(o => o.Detalles).AsNoTracking().FirstOrDefaultAsync(fa => fa.Id == id);
 if (f == null) return NotFound();
 return f;
 }

 [HttpPost]
 public async Task<ActionResult<Factura>> Post([FromBody] Factura factura)
 {
 // Calculate total from order detalles if not provided
 if (factura.OrdenServicioId !=0)
 {
 var detalles = await _db.DetallesOrden.Where(d => d.OrdenServicioId == factura.OrdenServicioId).ToListAsync();
 factura.Total = detalles.Sum(d => d.Subtotal);
 }

 _db.Facturas.Add(factura);
 await _db.SaveChangesAsync();
 return CreatedAtAction(nameof(Get), new { id = factura.Id }, factura);
 }
 }
}
