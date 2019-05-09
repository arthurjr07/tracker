using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DeviceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IDeviceBusiness _deviceBusiness;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="deviceBusiness">The Device Business Object</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="logger">The logger</param>
        public DevicesController(IDeviceBusiness deviceBusiness, 
                                 IMapper mapper, 
                                 ILogger<DevicesController> logger)
        {
            this._deviceBusiness = deviceBusiness;
            this._mapper = mapper;
            this._logger = logger;
        }


        // GET api/devices
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DeviceDTO>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> Get()
        {
            try
            {
                var devices = await _deviceBusiness.GetAllDevicesAsync().ConfigureAwait(false);
                var devicesDto = _mapper.Map<IEnumerable<DeviceDTO>>(devices);
                return Ok(devicesDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/devices/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(DeviceDTO))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            try
            {
                var device = await _deviceBusiness.GetDeviceByIdAsync(id).ConfigureAwait(false);
                return Ok(device);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // Search api/devices
        [HttpGet]
        [Route("Search")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return BadRequest();
            }

            try
            {
                var devices = await _deviceBusiness.SearchDevicesAsync(searchText).ConfigureAwait(false);
                var devicesDto = _mapper.Map<IEnumerable<DeviceDTO>>(devices);
                return Ok(devicesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET api/devices
        [HttpGet]
        [Authorize]
        [Route("GetHistory")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<IEnumerable<Log>>> GetHistory(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            try
            {
                var logs = await _deviceBusiness.GetHistoryAsync(id).ConfigureAwait(false);
                var logsDto = _mapper.Map<IEnumerable<LogDTO>>(logs);
                return Ok(logsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }


        // POST api/devices
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<DeviceDTO>> Register([FromBody] RegisterDeviceDTO value)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var device = _mapper.Map<Device>(value);
                var newDevice = await _deviceBusiness.RegisterAsync(device).ConfigureAwait(false);
                var deviceDto = _mapper.Map<DeviceDTO>(newDevice);
                return Ok(deviceDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        // DELETE api/devices/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult> UnRegister(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            try
            {
                var devices = await _deviceBusiness.UnRegisterAsync(id).ConfigureAwait(false);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
