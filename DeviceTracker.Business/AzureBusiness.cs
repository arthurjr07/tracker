using DeviceTracker.Business.DTO;
using DeviceTracker.Business.Interfaces;
using DeviceTracker.Domain.Entity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeviceTracker.Business
{
    public class AzureBusiness : IAzureBusiness
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptions<AzureSettings> azureSettings;
        private readonly ITokenService _tokenService;

        public AzureBusiness(IHttpClientFactory httpClientFactory,
            IOptions<AzureSettings> azureSettings,
            ITokenService tokenService)
        {
            this.httpClientFactory = httpClientFactory;
            this.azureSettings = azureSettings;
            this._tokenService = tokenService;
        }

        public async Task<UserInfo> Login(LoginDTO login)
        {
            var userInfo = default(UserInfo);
            using (var httpClient = httpClientFactory.CreateClient())
            using (var request = new FormUrlEncodedContent(CreateKeyValuePairCollection(login)))
            {
                var response = await httpClient.PostAsync(azureSettings.Value.LoginUrl, request);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var azureToken = JsonConvert.DeserializeObject<AzureToken>(content);

                userInfo = _tokenService.ReadToken(azureToken.access_token);
            }
            return userInfo;
        }

        private IEnumerable<KeyValuePair<string, string>> CreateKeyValuePairCollection(LoginDTO login)
        {
            return new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("resource", azureSettings.Value.Resource),
                new KeyValuePair<string, string>("grant_type", azureSettings.Value.GrantType),
                new KeyValuePair<string, string>("client_id", azureSettings.Value.ClientId),
                new KeyValuePair<string, string>("username", login.Email),
                new KeyValuePair<string, string>("password", login.Password)
            };
        }
    }
}
