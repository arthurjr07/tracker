using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Data;
using DeviceTracker.Domain.Enums;
using DeviceTracker.Domain.Models;

namespace DeviceTracker.Business
{
    /// <summary>
    /// Business object that deals with Log entity
    /// </summary>
    public class DeviceBusiness : IDeviceBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of <see cref="DeviceBusiness"/> class
        /// </summary>
        public DeviceBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await unitOfWork.DeviceRepository.GetAsync(c => !c.IsDeleted).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Device>> SearchDevicesAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return await unitOfWork.DeviceRepository.GetAsync(c => !c.IsDeleted).ConfigureAwait(false);
            }

            var properties = typeof(Device).GetProperties().Where(prop => prop.PropertyType == typeof(string)); ;
            var devices = await unitOfWork.DeviceRepository.GetAsync(c => !c.IsDeleted).ConfigureAwait(false);
            return devices.Where(d => properties.Any(prop =>
                                {
                                    var x = prop.GetValue(d, null)?.ToString().ToUpperInvariant().Contains(searchText.ToUpperInvariant());
                                    return x.HasValue ? x.Value : false;
                                }));
        }

        public async Task<IEnumerable<Log>> GetHistoryAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var device = await unitOfWork.DeviceRepository.FindByIdAsync(id).ConfigureAwait(false);
            if (device == null || device.IsDeleted)
            {
                return new List<Log>();
            }

            return await unitOfWork.LogRepository.GetHistoryByDeviceIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Device> RegisterAsync(Device newDevice)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(newDevice.Id).ConfigureAwait(false);
            if (device != null)
            {
                device.DeviceName = newDevice.DeviceName;
                device.OperatingSystem = newDevice.OperatingSystem;
                device.Version = newDevice.Version;
                await unitOfWork.DeviceRepository.UpdateAsync(device.Id, device).ConfigureAwait(false);
            }
            else
            {
                device = await unitOfWork.DeviceRepository.AddAsync(newDevice).ConfigureAwait(false);
            }

            await unitOfWork.CompleteAsync().ConfigureAwait(false);

            return device;
        }


        public async Task<bool> UnRegisterAsync(string id)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(id).ConfigureAwait(false);
            if (device == null)
            {
                return false;
            }

            device.IsDeleted = true;
            await unitOfWork.DeviceRepository.UpdateAsync(id, device).ConfigureAwait(false);
            await unitOfWork.CompleteAsync().ConfigureAwait(false);
            return true;
        }
    }
}
