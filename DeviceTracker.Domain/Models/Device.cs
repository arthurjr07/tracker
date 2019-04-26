using System;

namespace DeviceTracker.Domain.Models
{
    public class Device
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string CurrentUser { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
    }
}
