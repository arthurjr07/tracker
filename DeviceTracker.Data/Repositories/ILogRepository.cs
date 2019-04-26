using DeviceTracker.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeviceTracker.Data.Repositories
{
    public interface ILogRepository : IRepository<Log>
    {
        /// <summary>
        /// Get item by id in the repository
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<Log>> GetHistoryByDeviceIdAsync(string id);
    }
}