using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceTracker.Data.Repositories
{
    public class LogRepository : Repository<Log>, ILogRepository
    {
        public LogRepository(DeviceTrackerContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Log>> GetHistoryByDeviceIdAsync(string id)
        {
            var result = DbSet.Where(c => c.DeviceId == id);
            return await result.ToListAsync().ConfigureAwait(false);
        }
    }
}
