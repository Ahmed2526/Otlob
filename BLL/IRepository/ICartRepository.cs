using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface ICartRepository
    {
        Task<Cart> GetCustomerCartAsync(string cartId);
        Task<Cart> UpdateCustomerCartAsync(Cart cart);
        Task<bool> DeleteCartAsync(string cartId);
    }
}
