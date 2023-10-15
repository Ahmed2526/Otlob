using DAL.Dto_s;
using DAL.Entities;

namespace BLL.IRepository
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Pagination<Product>> GetAllWithSpec(string search, int? pageIndex, int? pageSize, int? typeId = null, int? brandId = null, string orderBy = null, string direction = null, string[] includes = null);


    }
}
