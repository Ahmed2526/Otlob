using DAL.Dto_s;

namespace BLL.IService
{
    public interface IProductService
    {
        Task<IEnumerable<productDto>> GetProducts();
        Task<Pagination<productDto>> GetProducts(string search, int? pageIndex, int? pageSize, int? typeId, int? brandId, string orderBy, string direction);
        Task<productDto> GetProduct(int id);
    }
}
