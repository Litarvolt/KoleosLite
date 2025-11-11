using Microsoft.EntityFrameworkCore;
using KoleosDemo.Entidades;

namespace KoleosDemo.Repositories
{
 public interface IRepository<T> where T : class
 {
 Task<IEnumerable<T>> GetAllAsync();
 Task<T?> GetByIdAsync(int id);
 Task AddAsync(T entity);
 Task UpdateAsync(T entity);
 Task DeleteAsync(int id);
 }

 public class GenericRepository<T> : IRepository<T> where T : class
 {
 private readonly DbContext _db;
 public GenericRepository(DbContext db) { _db = db; }
 public async Task<IEnumerable<T>> GetAllAsync() => await _db.Set<T>().AsNoTracking().ToListAsync();
 public async Task<T?> GetByIdAsync(int id) => await _db.Set<T>().FindAsync(id);
 public async Task AddAsync(T entity) { _db.Set<T>().Add(entity); await _db.SaveChangesAsync(); }
 public async Task UpdateAsync(T entity) { _db.Set<T>().Update(entity); await _db.SaveChangesAsync(); }
 public async Task DeleteAsync(int id) { var e = await GetByIdAsync(id); if (e != null) _db.Set<T>().Remove(e); await _db.SaveChangesAsync(); }
 }
}
