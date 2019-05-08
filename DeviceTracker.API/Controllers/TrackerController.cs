using AutoMapper;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DeviceTracker.API.Controllers
{
    [Route("api/[controller]")]
    public class TrackerController : Controller
    {
        private readonly IAzureBusiness _azureBusiness;
        private readonly ITrackerBusiness _trackerBusiness;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;


        public TrackerController(IAzureBusiness azureBusiness,
                                 ITrackerBusiness trackerBusiness,
                                 IMapper mapper,
                                 ILogger<AuthenticationController> logger)
        {
            this._azureBusiness = azureBusiness;
            this._trackerBusiness = trackerBusiness;
            this._mapper = mapper;
            this._logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CheckIn")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(UserInfo))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckIn([FromBody]CheckInDTO checkInDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var loginDto = _mapper.Map<LoginDTO>(checkInDTO);
                var userInfo = await _azureBusiness.Login(loginDto).ConfigureAwait(false);
                if (userInfo == null)
                {
                    return Unauthorized();
                }

                var checkInResult = await _trackerBusiness.CheckInAsync(checkInDTO, userInfo).ConfigureAwait(false);
                if (!checkInResult)
                {
                    return BadRequest(new { Message = "Unable to checkin device." });
                }

                return Ok(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("CheckOut")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckOut([FromBody]CheckOutDTO checkOutDTO)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var checkInResult = await _trackerBusiness.CheckOutAsync(checkOutDTO).ConfigureAwait(false);
                if (!checkInResult)
                {
                    return BadRequest(new { Message = "Unable to checkout device." });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return  StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}