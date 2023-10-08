using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.baseEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
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


        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

    }
}
