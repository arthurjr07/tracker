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

        public async Task<IEnumerable<Log>> GetHistoryAsync(string id)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(id).ConfigureAwait(false);
            if (device == null || device.IsDeleted)
            {
                throw new ApplicationException("Device does not exists.");
            }

            return await unitOfWork.LogRepository.GetHistoryByDeviceIdAsync(id).ConfigureAwait(false);
        }

        public async Task<Device> RegisterAsync(Device newDevice)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(newDevice.Id).ConfigureAwait(false);
            if (device != null)
            {
                throw new ApplicationException("Device already exists.");
            }
            device =  await unitOfWork.DeviceRepository.AddAsync(newDevice).ConfigureAwait(false);
            await unitOfWork.CompleteAsync().ConfigureAwait(false);

            return device;
        }

        public async Task<bool> UnRegisterAsync(string id)
        {
            bool result = true;
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(id).ConfigureAwait(false);
            if (device != null)
            {
                result =  false;
            }
            device.IsDeleted = true;
            await unitOfWork.DeviceRepository.UpdateAsync(id, device).ConfigureAwait(false);
            await unitOfWork.CompleteAsync().ConfigureAwait(false);
            return result;
        }
    }
}
