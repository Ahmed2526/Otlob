using AutoMapper;
using BLL.Interfaces;
using DAL.Consts;
using DAL.Dto_s;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Otlob_API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IRepository<Product> _Prodrepo;
        private readonly IMapper _mapper;

        public ProductsController(IRepository<Product> repository, IMapper mapper)
        {
            _Prodrepo = repository;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _Prodrepo.GetAllWithSpecAsync(new string[] { Entity.ProductBrand, Entity.ProductType });

            if (products is null)
                return NotFound();

            #region ManualMapping
            //var productDTo = products.Select(e => new productDto
            //{
            //    Id = e.Id,
            //    Name = e.Name,
            //    Description = e.Description,
            //    PictureUrl = e.PictureUrl,
            //    Price = e.Price,
            //    ProductBrand = e.ProductBrand!.Name,
            //    ProductType = e.ProductType!.Name
            //});
            #endregion

            var productDTo = _mapper.Map<List<productDto>>(products);

            return Ok(productDTo);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _Prodrepo.GetByIdAsync(id, new string[] { Entity.ProductBrand, Entity.ProductType });

            if (product is null)
                return NotFound();

            #region ManualMapping
            //var productDto = new productDto()
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    PictureUrl = product.PictureUrl,
            //    Price = product.Price,
            //    ProductBrand = product.ProductBrand!.Name,
            //    ProductType = product.ProductType!.Name
            //};
            #endregion

            var productDto = _mapper.Map<productDto>(product);

            return Ok(productDto);
        }




    }
}
