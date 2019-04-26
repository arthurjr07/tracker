using DeviceTracker.Data.EntityConfigurations;
using DeviceTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;

namespace DeviceTracker.Data
{
    public class DeviceTrackerContext : DbContext
    {
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        
        public DeviceTrackerContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(DeviceConfiguration)));
        }
    }
}
