using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Entity;
using System.Threading.Tasks;

namespace DeviceTracker.Business.Interfaces
{
    public interface IAzureBusiness
    {
        Task<bool> Login(LoginDTO login);
    }
}
