using HotelManager.Application.CustomException;
using HotelManager.Application.IRepository;
using HotelManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelManager.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Remove(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<T> GetById(int id)
        {
            var exist = await _dbSet.FindAsync(id);
            if(exist == null) throw new NotExistsException("Not exist");
            return exist;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<T>> GetAllOrder(
    Expression<Func<T, object>> orderBy = null)
        {
            IQueryable<T> query = _dbSet;

            if (orderBy != null)
                query = query.OrderBy(orderBy);

            return await query.ToListAsync();
        }
    }
}
