using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SDBlog.BusinessLayer.Interfaces.Base
{
    public interface IFileBaseRepository<T> where T : class
    {
        /// <summary>
        /// Creates entity in database.
        /// </summary>
        /// <param name="entity">Entity to create</param>
        /// <returns>Returns created entity.</returns>
        Task<T> Create(T entity);

        Task<T> FindAsync(Expression<Func<T, bool>> condition, params Expression<Func<T, object>>[] includes);

        Task SaveAsync();
    }
}