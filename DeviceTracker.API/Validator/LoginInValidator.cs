using DeviceTracker.Business.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.Validation
{
    /// <summary>
    /// Validator class for LoginDTO
    /// </summary>
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LoginValidator()
        {
            RuleFor(reg => reg.Email).NotEmpty();
            RuleFor(reg => reg.Password).NotEmpty();
        }
    }
}
