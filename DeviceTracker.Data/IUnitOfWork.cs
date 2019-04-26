using System.Threading.Tasks;
using DeviceTracker.Data.Repositories;

namespace DeviceTracker.Data
{
    public interface IUnitOfWork
    {
        IDeviceRepository DeviceRepository { get; }
        ILogRepository LogRepository { get; }
        
        Task CompleteAsync();
    }
}
