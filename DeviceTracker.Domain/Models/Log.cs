using System;

namespace DeviceTracker.Domain.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string UserEmail { get; set; }
        public int LogType { get; set; }
        public string DeviceId { get; set; }
    }
}
