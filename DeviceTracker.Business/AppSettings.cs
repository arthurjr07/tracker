using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceTracker.Business
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public IEnumerable<string> ApplicationUsers { get; set; }
    }
}
