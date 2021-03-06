using System;
using System.Linq;
using System.Threading.Tasks;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Data;
using DeviceTracker.Domain.Entity;
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
        public async Task<bool> CheckInAsync(CheckInDTO checkIn, UserInfo userInfo)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(checkIn.Id);
            if (device == null || device.IsDeleted)
            {
                return false;
            }

            if(!string.IsNullOrWhiteSpace(device.CurrentUser)) //device is currently in use.
            {
                var logOut = new Log
                {
                    LogTime = DateTime.Now,
                    LogType = (int)LogType.CheckOut,
                    UserName = device.CurrentUser,
                    DeviceId = device.Id
                };
                await unitOfWork.LogRepository.AddAsync(logOut).ConfigureAwait(false);
            }

            device.CurrentUser = userInfo.Name;
            device.Remarks = checkIn.Remarks;
            await unitOfWork.DeviceRepository.UpdateAsync(device.Id, device).ConfigureAwait(false);

            var log = new Log
            {
                LogTime = DateTime.Now,
                LogType = (int)LogType.CheckIn,
                UserName = userInfo.Name,
                DeviceId = device.Id
            };

            await unitOfWork.LogRepository.AddAsync(log).ConfigureAwait(false);
            await unitOfWork.CompleteAsync().ConfigureAwait(false);

            return true;
        }


        /// <summary>
        /// Checkout a device
        /// </summary>
        /// <param name="checkOut"></param>
        /// <returns></returns>
        public async Task<bool> CheckOutAsync(CheckOutDTO checkOut)
        {
            var device = await unitOfWork.DeviceRepository.FindByIdAsync(checkOut.Id);
            if (device == null || device.IsDeleted)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(device.CurrentUser))
            {
                var log = new Log
                {
                    LogTime = DateTime.Now,
                    LogType = (int)LogType.CheckOut,
                    UserName = device.CurrentUser,
                    DeviceId = device.Id
                };
                await unitOfWork.LogRepository.AddAsync(log).ConfigureAwait(false);
            }

            device.CurrentUser = string.Empty;
            device.Remarks = string.Empty;
            await unitOfWork.DeviceRepository.UpdateAsync(device.Id, device).ConfigureAwait(false);


            await unitOfWork.CompleteAsync().ConfigureAwait(false);

            return true;
        }
    }
}
