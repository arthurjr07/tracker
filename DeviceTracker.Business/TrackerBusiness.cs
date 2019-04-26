using System;
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
    public class TrackerBusiness : ITrackerBusiness
    {
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of <see cref="LogBusiness"/> class
        /// </summary>
        public TrackerBusiness(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Checked in a device
        /// </summary>
        /// <param name="checkIn">Contains the check in information</param>
        /// <returns>Returns true if the device is succesfully checked in, otherwise false</returns>
        public async Task<bool> CheckInAsync(CheckInDTO checkIn)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(checkIn.Id);
            if (device == null || device.IsDeleted)
            {
                return false;
            }

            device.CurrentUser = checkIn.Email;
            device.Remarks = checkIn.Remarks;
            await unitOfWork.DeviceRepository.UpdateAsync(device.Id, device).ConfigureAwait(false);

            var log = new Log
            {
                LogTime = DateTime.Now,
                LogType = (int)LogType.CheckIn,
                UserEmail = checkIn.Email,
                DeviceId = device.Id
            };

            await unitOfWork.LogRepository.AddAsync(log).ConfigureAwait(false);
            await unitOfWork.CompleteAsync().ConfigureAwait(false);

            return true;
        }

        public async Task<bool> CheckOutAsync(CheckOutDTO checkOut)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(checkOut.Id);
            if (device == null || device.IsDeleted)
            {
                return false;
            }

            device.CurrentUser = string.Empty;
            device.Remarks = string.Empty;
            await unitOfWork.DeviceRepository.UpdateAsync(device.Id, device).ConfigureAwait(false);

            var log = new Log
            {
                LogTime = DateTime.Now,
                LogType = (int)LogType.CheckOut,
                UserEmail = checkOut.Email,
                DeviceId = device.Id
            };

            await unitOfWork.LogRepository.AddAsync(log).ConfigureAwait(false);
            await unitOfWork.CompleteAsync().ConfigureAwait(false);

            return true;
        }
    }
}