using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Domain.Entity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeviceTracker.Business
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;

        public TokenService(IOptions<AppSettings> appSettings)
        {
            this._appSettings = appSettings.Value;
        }

        public UserInfo ReadToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var name = jwtToken.Claims.First(claim => claim.Type == "name").Value;
            var fname = jwtToken.Claims.First(claim => claim.Type == "family_name").Value;
            var lname = jwtToken.Claims.First(claim => claim.Type == "given_name").Value;
            var email = jwtToken.Claims.First(claim => claim.Type == "upn").Value;
            return new UserInfo()
            {
                Email = email,
                FamilyName = lname,
                GivenName = fname,
                Name = name
            };
        }

        public string RetrieveToken(UserInfo userInfo)
        {
            var accessToken = default(string);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userInfo.Name),
                    new Claim(ClaimTypes.Email, userInfo.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            accessToken = tokenHandler.WriteToken(token);

            return accessToken;
        }
    }
}
