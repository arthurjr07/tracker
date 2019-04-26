using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Models;

namespace DeviceTracker.Business.Interfaces
{
    public interface IDeviceBusiness
    {
        Task<Device> RegisterAsync(Device device);

        Task<bool> UnRegisterAsync(string id);

        Task<IEnumerable<Device>> GetAllDevicesAsync();

        Task<IEnumerable<Log>> GetHistoryAsync(string id);
    }
}
