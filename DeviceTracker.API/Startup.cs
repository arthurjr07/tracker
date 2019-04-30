using AutoMapper;
using DeviceTracker.Business;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Business.Mapper;
using DeviceTracker.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Threading.Tasks;
using DeviceTracker.Domain.Models;
using System;
using Swashbuckle.AspNetCore.Swagger;

namespace DeviceTracker.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAutoMapper(Assembly.GetAssembly(typeof(LoginProfile)));

            services.AddDbContext<DeviceTrackerContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DeviceTracker")));

            services.Configure<AzureSettings>(Configuration.GetSection("AzureSettings"));
            services.AddHttpClient();
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAzureBusiness, AzureBusiness>();
            services.AddScoped<IDeviceBusiness, DeviceBusiness>();
            services.AddScoped<ITrackerBusiness, TrackerBusiness>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample Project API V1");
            });

            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
