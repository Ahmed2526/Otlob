using BLL.IRepository;
using DAL.Data;
using DAL.Dto_s;
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

        public async Task<Pagination<Product>> GetAllWithSpec(string search, int? pageIndex, int? pageSize, int? typeId = null, int? brandId = null, string orderBy = null, string direction = null, string[] includes = null)
        {
            IQueryable<Product> query = _context.Set<Product>();

            int pageindex = (int)(pageIndex.HasValue ? pageIndex : 1);
            int pagesize = (int)(pageSize.HasValue ? pageSize : 10);

            int take = pagesize;
            int skip = pagesize * (pageindex - 1);


            if (!string.IsNullOrEmpty(search))
                query = query.Where(e => e.Name.Contains(search));

            if (typeId.HasValue)
                query = query.Where(e => e.ProductTypeId == typeId);

            if (brandId.HasValue)
                query = query.Where(e => e.ProductBrandId == brandId);


            if (!string.IsNullOrEmpty(orderBy))
            {
                if (!string.IsNullOrEmpty(direction))
                    query = query.OrderBy($"{orderBy} {direction}");

                else
                    query = query.OrderBy($"{orderBy} ASC");
            }

            int count = query.Count();

            query = query.Skip(skip).Take(take);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            var result = new Pagination<Product>()
            {
                pageIndex = pageindex,
                pageSize = pagesize,
                count = count,
                data = await query.ToListAsync()
            };

            return result;
        }
    }
}
