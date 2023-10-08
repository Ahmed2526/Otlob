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

        public ProductsController(IRepository<Product> repository)
        {
            _Prodrepo = repository;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _Prodrepo.GetAllWithSpecAsync(new string[] { Entity.ProductBrand, Entity.ProductType });

            if (products is null)
                return NotFound();

            var productsDto = products.Select(e => new productDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                PictureUrl = e.PictureUrl,
                Price = e.Price,
                ProductBrand = e.ProductBrand!.Name,
                ProductType = e.ProductType!.Name
            });

            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _Prodrepo.GetByIdAsync(id, new string[] { Entity.ProductBrand, Entity.ProductType });

            if (product is null)
                return NotFound();

            var productDto = new productDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                ProductBrand = product.ProductBrand!.Name,
                ProductType = product.ProductType!.Name
            };

            return Ok(productDto);
        }



    }
}
