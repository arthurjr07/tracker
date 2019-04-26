using System.Threading.Tasks;
using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Entity;

namespace DeviceTracker.Business.Interfaces
{
    public interface ITrackerBusiness
    {
        Task<bool> CheckInAsync(CheckInDTO checkin);

        Task<bool> CheckOutAsync(CheckOutDTO checkout);
    }
}
