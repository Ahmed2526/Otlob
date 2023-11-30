using BLL.IRepository;
using BLL.IService;
using BLL.Service;
using DAL.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IDatabase _database;
        private readonly ILoggerManager _logger;

        public CartRepository(IConnectionMultiplexer redis, ILoggerManager logger)
        {
            _database = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<Cart> GetCustomerCartAsync(string cartId)
        {
            try
            {
                var basket = await _database.StringGetAsync(cartId);

                if (!basket.HasValue)
                {
                    var newCart = await UpdateCustomerCartAsync(new Cart() { Id = cartId });
                    return newCart;
                }
                var cart = JsonSerializer.Deserialize<Cart>(basket!);
                return cart;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(CartRepository)} in {nameof(GetCustomerCartAsync)} method {ex}");
                throw;
            }
        }

        public async Task<Cart> UpdateCustomerCartAsync(Cart cart)
        {
            try
            {
                if (cart.Id is null)
                {
                    var key = Guid.NewGuid().ToString();
                    var create = await _database.StringSetAsync(key, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
                    if (create)
                    {
                        cart.Id = key;
                        return cart;
                    }
                }
                var set = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
                if (set)
                    return cart;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(CartRepository)} in {nameof(UpdateCustomerCartAsync)} method {ex}");
                throw;
            }
            return null;
        }

        public async Task<bool> DeleteCartAsync(string cartId)
        {
            try
            {
                var result = await _database.KeyDeleteAsync(cartId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(CartRepository)} in {nameof(DeleteCartAsync)} method {ex}");
                throw;
            }
        }

    }
}
