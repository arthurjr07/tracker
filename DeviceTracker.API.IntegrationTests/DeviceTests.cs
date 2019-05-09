using DeviceTracker.Business.DTO;
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
    public class DeviceTests : IClassFixture<CustomWebApplicationFactory<DeviceTracker.API.Startup>>
    {
        private readonly CustomWebApplicationFactory<DeviceTracker.API.Startup> _factory;
        private string token = string.Empty;

        public DeviceTests(CustomWebApplicationFactory<DeviceTracker.API.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Execute_API_In_Correct_Order()
        {
            await InsertNewDevice().ConfigureAwait(false);
            await GetDeviceById().ConfigureAwait(false);
            await GetAllDevices().ConfigureAwait(false);
            await SearchDevice().ConfigureAwait(false);

            await LoginAsAdmin().ConfigureAwait(false);
            await GetHistory().ConfigureAwait(false);

            await DeleteDevice().ConfigureAwait(false);
            await ConfirmDelete().ConfigureAwait(false);
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

        private async Task GetAllDevices()
        {
            // Arrange
            var client = _factory.CreateClient();
            var getAllDevicesUrl = "/api/Devices";

            // Act
            var response = await client.GetAsync(getAllDevicesUrl).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var devices = JsonConvert.DeserializeObject<IEnumerable<DeviceDTO>>(responseContent);
            devices.Should().HaveCount(1);
            devices.First().Id.Should().Be("123456789");
        }

        private async Task SearchDevice()
        {
            // Arrange
            var client = _factory.CreateClient();
            var searchDeviceUrl = "/api/Devices?searchText=IPhone";

            // Act
            var response = await client.GetAsync(searchDeviceUrl).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var devices = JsonConvert.DeserializeObject<IEnumerable<DeviceDTO>>(responseContent);
            devices.Should().HaveCount(1);
            devices.First().Id.Should().Be("123456789");
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
            devices.Should().HaveCount(0);
        }

        private async Task DeleteDevice()
        {
            // Arrange
            var client = _factory.CreateClient();
            var getDeviceUrl = "/api/Devices/123456789";

            // Act
            var response = await client.DeleteAsync(getDeviceUrl).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task ConfirmDelete()
        {
            // Arrange
            var client = _factory.CreateClient();
            var getAllDevicesUrl = "/api/Devices";

            // Act
            var response = await client.GetAsync(getAllDevicesUrl).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var devices = JsonConvert.DeserializeObject<IEnumerable<DeviceDTO>>(responseContent);
            devices.Should().HaveCount(0);
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
