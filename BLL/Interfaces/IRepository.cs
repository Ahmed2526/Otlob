using DAL.Entities.baseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id, string[] includes = null);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithSpecAsync(string[] includes = null);
        int SaveChanges();
    }
}
