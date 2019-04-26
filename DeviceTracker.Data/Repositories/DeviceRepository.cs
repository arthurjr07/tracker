using DeviceTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DeviceTracker.Data.Repositories
{
    public class DeviceRepository : Repository<Device>, IDeviceRepository
    {
        public DeviceRepository(DeviceTrackerContext context) : base(context)
        {
        }
    }
}
