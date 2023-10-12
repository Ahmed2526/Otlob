using BLL.IRepository;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BLL.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithSpec(int? pageIndex, int? pageSize, int? typeId = null, int? brandId = null, string orderBy = null, string direction = null, string[] includes = null)
        {
            IQueryable<Product> query = _context.Set<Product>();

            int pageindex = (int)(pageIndex.HasValue ? pageIndex : 1);
            int pagesize = (int)(pageSize.HasValue ? pageSize : 10);

            int take = pagesize;
            int skip = pagesize * (pageindex - 1);

            query = query.Skip(skip).Take(take);

            if (typeId.HasValue)
                query = query.Where(e => e.ProductTypeId == typeId);

            if (brandId.HasValue)
                query = query.Where(e => e.ProductBrandId == brandId);

            if (!string.IsNullOrEmpty(orderBy))
                query = query.OrderBy($"{orderBy} {direction}");

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }
    }
}
