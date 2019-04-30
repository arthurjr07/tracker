using AutoMapper;
using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Models;

namespace DeviceTracker.Business.Mapper
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<CheckInDTO, LoginDTO>();
            CreateMap<RegisterDeviceDTO, Device>()
                .ForMember( c => c.CurrentUser, x => x.MapFrom(src => string.Empty))
                .ForMember(c => c.Remarks, x => x.MapFrom(src => string.Empty))
                .ForMember(c => c.IsDeleted , x => x.MapFrom(src => false));

            CreateMap<Log, LogDTO>()
                .ForMember(c => c.LogType, x => x.MapFrom(src => src.LogType == 0? "CheckIn" : "CheckOut"));
        }
    }
}
