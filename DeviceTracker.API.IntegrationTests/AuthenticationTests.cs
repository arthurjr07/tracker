using DeviceTracker.Business.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeviceTracker.API.IntegrationTests
{
    public class AuthenticationTests : IClassFixture<WebApplicationFactory<DeviceTracker.API.Startup>>
    {
        private readonly WebApplicationFactory<DeviceTracker.API.Startup> _factory;

        public AuthenticationTests(WebApplicationFactory<DeviceTracker.API.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_Return_BadRequest_When_Model_Is_InValid()
        {
            // Arrange
            var client = _factory.CreateClient();
            var loginUrl = "/api/Authentication/login";

            // Act
            var login = new LoginDTO() { Email = "", Password = "" };
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(loginUrl, content).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Should_Return_Ok_With_Token_When_UserName_And_Password_Is_Correct()
        {
            // Arrange
            var client = _factory.CreateClient();
            var loginUrl = "/api/Authentication/login";

            // Act
            var login = new LoginDTO() { Email = Credential.UserName, Password = Credential.Password };
            var json = JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(loginUrl, content).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var token = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            token.Should().NotBeNullOrWhiteSpace();
        }
    }
}
