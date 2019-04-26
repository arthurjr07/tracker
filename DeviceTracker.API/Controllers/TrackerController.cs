using AutoMapper;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeviceTracker.API.Controllers
{
    [Route("api/[controller]")]
    public class TrackerController : Controller
    {
        private readonly IAzureBusiness _azureBusiness;
        private readonly ITrackerBusiness _trackerBusiness;
        private readonly IMapper _mapper;

        public TrackerController(IAzureBusiness azureBusiness,
            ITrackerBusiness trackerBusiness,
            IMapper mapper)
        {
            this._azureBusiness = azureBusiness;
            this._trackerBusiness = trackerBusiness;
            this._mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("CheckIn")]
        public async Task<IActionResult> CheckIn([FromBody]CheckInDTO checkinDTO)
        {
            try
            {
                var loginDto = _mapper.Map<LoginDTO>(checkinDTO);
                var isValidCredential = await _azureBusiness.Login(loginDto).ConfigureAwait(false);
                if (!isValidCredential)
                {
                    return Unauthorized();
                }

                var checkInResult = await _trackerBusiness.CheckInAsync(checkinDTO).ConfigureAwait(false);
                if (!checkInResult)
                {
                    return BadRequest(new { Message = "Unable to checkin device." });
                }

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("CheckOut")]
        public async Task<IActionResult> CheckOut([FromBody]CheckOutDTO checkOutDTO)
        {
            try
            {
                var checkInResult = await _trackerBusiness.CheckOutAsync(checkOutDTO).ConfigureAwait(false);
                if (!checkInResult)
                {
                    return BadRequest(new { Message = "Unable to checkout device." });
                }

                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}