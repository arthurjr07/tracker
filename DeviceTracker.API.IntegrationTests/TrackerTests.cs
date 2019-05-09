using DeviceTracker.Business.DTO;
using DeviceTracker.Domain.Entity;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DeviceTracker.API.IntegrationTests
{
    public class TrackerTests : IClassFixture<CustomWebApplicationFactory<DeviceTracker.API.Startup>>
    {
        private readonly CustomWebApplicationFactory<DeviceTracker.API.Startup> _factory;
        private string token = string.Empty;

        public TrackerTests(CustomWebApplicationFactory<DeviceTracker.API.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Execute_API_In_Correct_Order()
        {
            await InsertNewDevice().ConfigureAwait(false);
            await GetDeviceById().ConfigureAwait(false);
            await CheckIn().ConfigureAwait(false);
            await CheckOut().ConfigureAwait(false);

            await LoginAsAdmin().ConfigureAwait(false);
            await GetHistory().ConfigureAwait(false);
        }

        private async Task InsertNewDevice()
        {
            // Arrange
            var client = _factory.CreateClient();
            var getAllDevicesUrl = "/api/Devices";

            // Act
            var device = new RegisterDeviceDTO()
            {
                Id = "123456789",
                DeviceName = "IPhone",
                OperatingSystem = "IOS 12.2",
                Version = "5S",
                ERNIControlNo = "00000"
            };

            var json = JsonConvert.SerializeObject(device);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(getAllDevicesUrl, content).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task GetDeviceById()
        {
            // Arrange
            var client = _factory.CreateClient();
            var getDeviceUrl = "/api/Devices/123456789";

            // Act
            var response = await client.GetAsync(getDeviceUrl).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var device = JsonConvert.DeserializeObject<DeviceDTO>(responseContent);
            device.Id.Should().Be("123456789");
        }

        private async Task CheckIn()
        {
            // Arrange
            var client = _factory.CreateClient();
            var checkInUrl = "/api/Tracker/CheckIn";

            // Act
            var checkIn = new CheckInDTO()
            {
                Id = "123456789", 
                Email = Credential.UserName, 
                Password = Credential.Password,
                Remarks = "testing"
            };
            var json = JsonConvert.SerializeObject(checkIn);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(checkInUrl, content).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var userInfo = JsonConvert.DeserializeObject<UserInfo>(responseContent);
            userInfo.Should().NotBeNull();
        }

        private async Task CheckOut()
        {
            // Arrange
            var client = _factory.CreateClient();
            var checkInUrl = "/api/Tracker/CheckOut";

            // Act
            var checkOut = new CheckOutDTO()
            {
                Id = "123456789"
            };
            var json = JsonConvert.SerializeObject(checkOut);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(checkInUrl, content).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task GetHistory()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var getHistoryUrl = "/api/Devices/GetHistory?id=123456789";

            // Act
            var response = await client.GetAsync(getHistoryUrl).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var devices = JsonConvert.DeserializeObject<IEnumerable<LogDTO>>(responseContent);
            devices.Should().HaveCount(2);
        }



        private async Task LoginAsAdmin()
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
            token = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            token.Should().NotBeNullOrWhiteSpace();
        }
    }
}
