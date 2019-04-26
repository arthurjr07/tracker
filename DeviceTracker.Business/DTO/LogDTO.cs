using System;

namespace DeviceTracker.Business.DTO
{
    public class LogDTO
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string UserEmail { get; set; }
        public string LogType { get; set; }
        public string DeviceId { get; set; }
    }
}
