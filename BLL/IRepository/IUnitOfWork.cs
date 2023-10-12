using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Product> Products { get; }
        IBaseRepository<ProductType> ProductTypes { get; }
        IBaseRepository<ProductBrand> ProductBrands { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
