namespace DeviceTracker.Business.DTO
{
    /// <summary>
    /// Data transfer object for checking in of device
    /// </summary>
    public class CheckInDTO : TrackingDTO
    {
        /// <summary>
        /// User email address
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }
    }
}
