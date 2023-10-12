using BLL.IRepository;
using DAL.Data;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<Product> Products { get; private set; }
        public IBaseRepository<ProductType> ProductTypes { get; private set; }
        public IBaseRepository<ProductBrand> ProductBrands { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Products = new BaseRepository<Product>(_context);
            ProductTypes = new BaseRepository<ProductType>(_context);
            ProductBrands = new BaseRepository<ProductBrand>(_context);
        }

        public void Dispose()
          => _context.Dispose();

        public int SaveChanges()
          => _context.SaveChanges();

        public async Task<int> SaveChangesAsync()
           => await _context.SaveChangesAsync();
    }
}
