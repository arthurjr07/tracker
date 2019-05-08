using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.DTO
{
    /// <summary>
    /// Data transfer object for checking out of devices
    /// </summary>
    public class CheckOutDTO : TrackingDTO
    {
        /// <summary>
        /// User email address
        /// </summary>
        public string UserName { get; set; }
    }
}
