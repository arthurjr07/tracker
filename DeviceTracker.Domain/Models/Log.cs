using System;

namespace DeviceTracker.Domain.Models
{
    /// <summary>
    /// Log class
    /// </summary>
    public class Log
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Date and time of the log
        /// </summary>
        public DateTime LogTime { get; set; }

        /// <summary>
        /// User name 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Type of log (e.g. Checkin, Checkout)
        /// </summary>
        public int LogType { get; set; }

        /// <summary>
        /// Device Id
        /// </summary>
        public string DeviceId { get; set; }
    }
}
