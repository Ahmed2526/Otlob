using AutoMapper;
using BLL.IRepository;
using BLL.IService;
using DAL.Consts;
using DAL.Dto_s;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob_API.ErrorModel;
using System.Net;

namespace Otlob_API.Controllers
{
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProduct(id);

            if (product is null)
                return NotFound(new ApiResponse((int)HttpStatusCode.NotFound));

            return Ok(product);
        }

        [HttpGet("error/{id}")]
        public ActionResult GetError(int id)
        {

            var atat = "ss";
            int ota = int.Parse(atat);

            return Ok();
        }

    }
}
