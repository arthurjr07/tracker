using DeviceTracker.Business.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.Validation
{
    public class CheckOutValidator : AbstractValidator<CheckOutDTO>
    {
        public CheckOutValidator()
        {
            RuleFor(reg => reg.UserName).NotEmpty();
            RuleFor(reg => reg.Id).NotEmpty();
        }
    }
}
