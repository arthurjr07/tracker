using System.Threading.Tasks;
using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Entity;

namespace DeviceTracker.Business.Interfaces
{
    public interface ITrackerBusiness
    {
        Task<bool> CheckInAsync(CheckInDTO checkIn, UserInfo userInfo);

        Task<bool> CheckOutAsync(CheckOutDTO checkOut);
    }
}
