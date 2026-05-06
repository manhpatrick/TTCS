using System.Linq.Expressions;

namespace HotelManager.Application.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        Task Update(int id, T entity);
        Task Remove(T entity);
        Task SaveAsync();
        Task<T> GetById(int id);

        Task<List<T>> GetAllOrder(Expression<Func<T, object>> orderBy);
    }
}
