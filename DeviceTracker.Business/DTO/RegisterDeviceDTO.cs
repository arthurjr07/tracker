using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business.DTO
{
    public class RegisterDeviceDTO
    {
        public string Id { get; set; }
        public string DeviceName { get; set; }
        public string OperatingSystem { get; set; }
        public string Version { get; set; }
        public string ERNIControlNo { get; set; }
    }
}
