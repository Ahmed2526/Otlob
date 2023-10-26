using AutoMapper;
using BLL.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Otlob_API.ErrorModel;
using System.Net;

namespace Otlob_API.Controllers
{
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;

        public ProductsController(IProductService productService, IMapper mapper, ILoggerManager loggerManager)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = loggerManager;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _productService.GetProducts();

            if (products is null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound));

            return Ok(products);
        }

        [HttpGet("GetAllWithSpecs")]
        public async Task<ActionResult> GetProducts(string? search, int? pageIndex, int? pageSize, int? typeId, int? brandId, string? orderBy, string? direction)
        {
            var products = await _productService.GetProducts(search, pageIndex, pageSize, typeId, brandId, orderBy, direction);

            if (products is null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound));

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);

            if (product is null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound));

            return Ok(product);
        }

    }
}
