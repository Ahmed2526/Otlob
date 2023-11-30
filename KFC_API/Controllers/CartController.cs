using BLL.IRepository;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Otlob_API.Controllers
{

    public class CartController : BaseApiController
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCustomerCart(string id)
        {
            var result = await _cartRepository.GetCustomerCartAsync(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCart([FromBody] Cart cart)
        {
            var result =await _cartRepository.UpdateCustomerCartAsync(cart);
            return Ok(result);
        }

        [HttpPost("DeleteCart/{id}")]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var result = await _cartRepository.DeleteCartAsync(id);
            return Ok(result);
        }

    }
}
