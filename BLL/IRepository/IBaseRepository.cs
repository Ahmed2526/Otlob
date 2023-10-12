using DAL.Entities.baseEntity;

namespace BLL.IRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id, string[] includes = null);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllWithSpecAsync(string[] includes = null);
        Task<IEnumerable<T>> GetAllWithSpecOrderedAsync(string orderBy, string direction, string[] includes = null);

    }
}
