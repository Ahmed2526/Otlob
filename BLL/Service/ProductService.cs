using AutoMapper;
using BLL.IRepository;
using BLL.IService;
using DAL.Consts;
using DAL.Dto_s;

namespace BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<productDto>> GetProducts()
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllWithSpecAsync(new string[] { Entity.ProductBrand, Entity.ProductType });

                if (products is null)
                    return null;

                var productDTo = _mapper.Map<List<productDto>>(products);

                return productDTo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(ProductService)} in {nameof(GetProducts)} method {ex}");
                throw;
            }
        }
        public async Task<Pagination<productDto>> GetProducts(string search, int? pageIndex, int? pageSize, int? typeId, int? brandId, string orderBy, string direction)
        {
            try
            {
                var products = await _unitOfWork.Products.GetAllWithSpec(search, pageIndex, pageSize, typeId, brandId, orderBy, direction, new string[] { Entity.ProductBrand, Entity.ProductType });

                if (products is null)
                    return null;

                var productDTo = _mapper.Map<List<productDto>>(products.data);

                var result = new Pagination<productDto>()
                {
                    pageIndex = products.pageIndex,
                    pageSize = products.pageSize,
                    count = products.count,
                    data = productDTo
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(ProductService)} in {nameof(GetProducts)} method {ex}");
                throw;
            }

        }

        public async Task<productDto> GetProduct(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id, new string[] { Entity.ProductBrand, Entity.ProductType });

                if (product is null)
                    return null;

                var productDto = _mapper.Map<productDto>(product);
                return productDto;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                    $" {nameof(ProductService)} in {nameof(GetProduct)} method {ex}");
                throw;
            }
        }


    }
}
