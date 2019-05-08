using DeviceTracker.Business.DTO;
using FluentValidation;


namespace DeviceTracker.Business.Validation
{
    class RegisterDeviceValidator : AbstractValidator<RegisterDeviceDTO>
    {
        public RegisterDeviceValidator()
        {
            RuleFor(reg => reg.Id).NotEmpty();
            RuleFor(reg => reg.DeviceName).NotEmpty();
            RuleFor(reg => reg.OperatingSystem).NotEmpty();
            RuleFor(reg => reg.Version).NotEmpty();
        }
    }
}
