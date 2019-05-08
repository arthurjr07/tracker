using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeviceTracker.Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        /// <summary>
        /// Add item to the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Delete item in the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateAsync(string id, TEntity updated);

        /// <summary>
        /// Delete item in the repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(string id);

        /// <summary>
        /// Get items from the repository
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get item by id in the repository
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<TEntity> FindByIdAsync(string id);
    }
}
