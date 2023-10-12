using DAL.Entities;

namespace BLL.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IBaseRepository<ProductType> ProductTypes { get; }
        IBaseRepository<ProductBrand> ProductBrands { get; }

        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
