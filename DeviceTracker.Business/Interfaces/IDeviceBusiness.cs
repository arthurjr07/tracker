using System.Collections.Generic;
using System.Threading.Tasks;
using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Models;

namespace DeviceTracker.Business.Interfaces
{
    public interface IDeviceBusiness
    {
        Task<Device> RegisterAsync(Device newDevice);

        Task<bool> UnRegisterAsync(string id);

        Task<Device> GetDeviceByIdAsync(string id);

        Task<IEnumerable<Device>> GetAllDevicesAsync();

        Task<IEnumerable<Log>> GetHistoryAsync(string id);

        Task<IEnumerable<Device>> SearchDevicesAsync(string searchText);
    }
}
