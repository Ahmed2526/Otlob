using BLL.IRepository;
using DAL.Data;
using DAL.Entities.baseEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BLL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllWithSpecAsync(string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecOrderedAsync(string orderBy, string direction, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderBy($"{orderBy} {direction}");

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public async Task<T> GetByIdAsync(int id, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>().Where(e => e.Id == id);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.FirstOrDefaultAsync();
        }

    }
}
