using System;

namespace DeviceTracker.Domain.Models
{
    /// <summary>
    /// Device class
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of the device
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Operating system of the device (e.g. IOS, Android)
        /// </summary>
        public string OperatingSystem { get; set; }

        /// <summary>
        /// Version of the device (e.g. 5S, S9)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Erni control number of the device. This is the number in erni sticker at the back of the device
        /// </summary>
        public string ERNIControlNo { get; set; }

        /// <summary>
        /// Current user
        /// </summary>
        public string CurrentUser { get; set; }

        /// <summary>
        /// Remarks of the current user
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Flag (soft delete)
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
