using DeviceTracker.Business.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.Validation
{
    public class CheckInValidator : AbstractValidator<CheckInDTO>
    {
        public CheckInValidator()
        {
            RuleFor(reg => reg.Id).NotEmpty();
            RuleFor(reg => reg.Email).NotEmpty();
            RuleFor(reg => reg.Password).NotEmpty();
        }
    }
}
