using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeviceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IDeviceBusiness _deviceBusiness;

        public DevicesController(IDeviceBusiness deviceBusiness, IMapper mapper)
        {
            this._deviceBusiness = deviceBusiness;
            this._mapper = mapper;
        }


        // GET api/devices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> Get()
        {
            try
            {
                var devices = await _deviceBusiness.GetAllDevicesAsync().ConfigureAwait(false);
                var devicesDto = _mapper.Map<IEnumerable<DeviceDTO>>(devices);
                return Ok(devicesDto);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // GET api/devices
        [HttpGet]
        [Route("GetHistory")]
        public async Task<ActionResult<IEnumerable<Log>>> GetHistory(string id)
        {
            try
            {
                var logs = await _deviceBusiness.GetHistoryAsync(id).ConfigureAwait(false);
                var logsDto = _mapper.Map<IEnumerable<LogDTO>>(logs);
                return Ok(logsDto);
            }
            catch
            {
                return StatusCode(500);
            }
        }


        // POST api/devices
        [HttpPut]
        public async Task<ActionResult<DeviceDTO>> Register([FromBody] RegisterDeviceDTO value)
        {
            try
            {
                var device = _mapper.Map<Device>(value);
                var newDevice = await _deviceBusiness.RegisterAsync(device).ConfigureAwait(false);
                var deviceDto = _mapper.Map<DeviceDTO>(newDevice);
                return Ok(deviceDto);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        // DELETE api/devices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> UnRegister(string id)
        {
            try
            {
                var devices = await _deviceBusiness.UnRegisterAsync(id).ConfigureAwait(false);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
