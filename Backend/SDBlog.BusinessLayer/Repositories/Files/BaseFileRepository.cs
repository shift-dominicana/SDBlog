using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using SDBlog.BusinessLayer.Interfaces.Base;
using SDBlog.DataModel.Context;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Boundaries.Database.Repositories
{
    public class BaseFileRepository<T> : IFileBaseRepository<T> where T : class
    {
        private readonly MainDbContext _mainDbContext;
        private readonly DbSet<T> _set;

        public BaseFileRepository(MainDbContext applicationDbContext)
        {
            _mainDbContext = applicationDbContext;
            _set = applicationDbContext.Set<T>();
        }
        async Task<T> IFileBaseRepository<T>.Create(T entity)
        {
            _set.Add(entity);
            await _mainDbContext.SaveChangesAsync();
            return entity;
        }

        async Task<T> IFileBaseRepository<T>.FindAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable = _set.AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                queryable = queryable.Include(include);
            }

            return await queryable.Where(condition).FirstOrDefaultAsync();
        }

        async Task IFileBaseRepository<T>.SaveAsync()
        {
            await _mainDbContext.SaveChangesAsync();
        }
    }
}
