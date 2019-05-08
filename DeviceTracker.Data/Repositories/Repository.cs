using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeviceTracker.Data.Repositories
{
    /// <summary>
    /// Base class for all repositories
    /// </summary>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly DbSet<TEntity> DbSet;

        /// <summary>
        /// Creates a new instance of <see cref="Repository{TEntity}"/> instance
        /// </summary>
        public Repository(DeviceTrackerContext context)
        {
            DbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Insert the entity in the database
        /// </summary>
        /// <param name="entity">Entity to insert</param>
        /// <returns></returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await DbSet.AddAsync(entity);
            return result.Entity;
        }

        /// <summary>
        /// Removes the entity to the database
        /// </summary>
        /// <param name="id">Id of the entity to delete</param>
        /// <returns></returns>
        public async Task DeleteAsync(string id)
        {
            var entity = await DbSet.FindAsync(id).ConfigureAwait(false);
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        /// <summary>
        /// Get a filtered set of the list with given predicate expression
        /// </summary>
        /// <param name="predicate">The query expression</param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = DbSet.Where(predicate);
            return await result.ToListAsync().ConfigureAwait(false);
        }

        public async Task<TEntity> FindByIdAsync(string id)
        {
            return await DbSet.FindAsync(id).ConfigureAwait(false);
        }

        public async Task UpdateAsync(string id, TEntity updated)
        {
            var entity = await DbSet.FindAsync(id).ConfigureAwait(false);
            if (entity != null)
            {
                DbSet.Update(updated);
            }
        }
    }
}
