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

        public AzureBusiness(IHttpClientFactory httpClientFactory,
            IOptions<AzureSettings> azureSettings)
        {
            this.httpClientFactory = httpClientFactory;
            this.azureSettings = azureSettings;
        }

        public async Task<bool> Login(LoginDTO login)
        {
            var result = false;
            try
            {
                using (var httpClient = httpClientFactory.CreateClient())
                using (var content = new FormUrlEncodedContent(CreateKeyValuePairCollection(login)))
                {
                    var response = await httpClient.PostAsync(azureSettings.Value.LoginUrl, content);

                    response.EnsureSuccessStatusCode();

                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
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
