using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Entity;
using System.Threading.Tasks;

namespace DeviceTracker.Business.Interfaces
{
    public interface ITokenService
    {
        string RetrieveToken(UserInfo userInfo);

        UserInfo ReadToken(string accessToken);
    }
}
