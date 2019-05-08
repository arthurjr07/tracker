using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DeviceTracker.Domain.Entity;
using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DeviceTracker.Business;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging;

namespace DeviceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAzureBusiness _azureBusiness;
        private readonly ITokenService _tokenService;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="azureBusiness">The Azure Business object</param>
        /// <param name="appSettings">The Application Settings object</param>
        /// <param name="tokenBusiness">The Token Service object</param>
        /// <param name="logger">The Logger object</param>
        public AuthenticationController(IAzureBusiness azureBusiness,
                                        IOptions<AppSettings> appSettings,
                                        ITokenService tokenService,
                                        ILogger<AuthenticationController> logger)
        {
            this._azureBusiness = azureBusiness;
            this._appSettings = appSettings.Value;
            this._tokenService = tokenService;
            this._logger = logger;
        }

        /// <summary>
        /// Login API
        /// </summary>
        /// <param name="credential">The username and password</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type=typeof(string))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDTO credential)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var applicationUsers = _appSettings.ApplicationUsers;

                if (!applicationUsers.Any(u => u.ToUpperInvariant() == credential.Email.ToUpperInvariant()))
                {
                    return BadRequest();
                }

                var userInfo = await _azureBusiness.Login(credential).ConfigureAwait(false);
                if (userInfo == null)
                {
                    return Unauthorized();
                }

                var accessToken = _tokenService.RetrieveToken(userInfo);

                return Ok(accessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
