using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Domain.Entity
{
    public class UserInfo
    {
        /// <summary>
        /// Full of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Surname of the user
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Firstname of the user
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Email { get; set; }
    }
}
