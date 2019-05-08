using DeviceTracker.Business.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.Validation
{
    /// <summary>
    /// Validator class for CheckOutDTO
    /// </summary>
    public class CheckOutValidator : AbstractValidator<CheckOutDTO>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CheckOutValidator()
        {
            RuleFor(reg => reg.Id).NotEmpty();
        }
    }
}
