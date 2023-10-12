using DAL.Dto_s;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IService
{
    public interface IProductService
    {
        Task<IEnumerable<productDto>> GetProducts();
        Task<productDto> GetProduct(int id);
    }
}
