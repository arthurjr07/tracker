using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.DTO
{
    /// <summary>
    /// Base data transfer object for tracking of device
    /// </summary>
    public class TrackingDTO
    {
        /// <summary>
        /// Unique identifier of the device
        /// </summary>
        public string Id { get; set; }
    }
}
