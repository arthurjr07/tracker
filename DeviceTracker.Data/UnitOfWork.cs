using System.Threading.Tasks;
using DeviceTracker.Data.Repositories;

namespace DeviceTracker.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDeviceRepository DeviceRepository => new DeviceRepository(context);
        public ILogRepository LogRepository => new LogRepository(context);

        private readonly DeviceTrackerContext context;

        public UnitOfWork(DeviceTrackerContext context)
        {
            this.context = context;
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
