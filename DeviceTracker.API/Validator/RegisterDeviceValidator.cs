using DeviceTracker.Business.DTO;
using FluentValidation;


namespace DeviceTracker.Business.Validation
{
    /// <summary>
    /// Validator class for RegisterDeviceDTO
    /// </summary>
    public class RegisterDeviceValidator : AbstractValidator<RegisterDeviceDTO>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RegisterDeviceValidator()
        {
            RuleFor(reg => reg.Id).NotEmpty();
            RuleFor(reg => reg.DeviceName).NotEmpty();
            RuleFor(reg => reg.OperatingSystem).NotEmpty();
            RuleFor(reg => reg.Version).NotEmpty();
        }
    }
}
