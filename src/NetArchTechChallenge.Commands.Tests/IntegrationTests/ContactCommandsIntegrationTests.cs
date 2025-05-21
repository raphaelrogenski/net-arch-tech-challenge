//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using Moq;
//using NetArchTechChallenge.Shared.Application.DTOs;
//using NetArchTechChallenge.Shared.Domain.Events;
//using System.Net;
//using System.Net.Http.Json;

//namespace NetArchTechChallenge.Commands.Tests.IntegrationTests
//{
//    public class ContactCommandsIntegrationTests : IClassFixture<CustomWebApplicationFactory>
//    {
//        private readonly HttpClient _client;

//        public ContactCommandsIntegrationTests(CustomWebApplicationFactory factory)
//        {
//            _client = factory.CreateClient();
//        }

//        [Fact]
//        public async Task Create_ShouldReturn204_WhenValid()
//        {
//            // Arrange
//            var request = new ContactDto
//            {
//                Name = "Raphael",
//                PhoneDDD = "42",
//                PhoneNumber = "999999999",
//                EmailAddress = "test@email.com",
//            };

//            // Act
//            var response = await _client.PostAsJsonAsync("/api/contacts", request);

//            // Assert
//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//        }

//        [Fact]
//        public async Task Update_ShouldReturn204_WhenValid()
//        {
//            // Arrange
//            var request = new ContactDto
//            {
//                Name = "Raphael",
//                PhoneDDD = "42",
//                PhoneNumber = "999999999",
//                EmailAddress = "test1@email.com",
//            };

//            await _client.PostAsJsonAsync("/api/contacts", request);

//            request.EmailAddress = "test2@email.com";

//            // Act
//            var response = await _client.PutAsJsonAsync("/api/contacts", request);

//            // Assert
//            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//        }

//        [Fact]
//        public async Task Create_ShouldReturn400_WhenInvalid()
//        {
//            // Arrange
//            var request = new ContactDto
//            {
//                Name = "",
//                PhoneDDD = "",
//                PhoneNumber = "",
//                EmailAddress = "",
//            };

//            // Act
//            var response = await _client.PostAsJsonAsync("/api/contacts", request);

//            // Assert
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//        }

//        [Fact]
//        public async Task Update_ShouldReturn400_WhenInvalid()
//        {
//            // Arrange
//            var request = new ContactDto
//            {
//                Name = "",
//                PhoneDDD = "",
//                PhoneNumber = "",
//                EmailAddress = "",
//            };

//            // Act
//            var response = await _client.PutAsJsonAsync("/api/contacts", request);

//            // Assert
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//        }

//        [Fact]
//        public async Task Delete_ShouldReturn400_WhenInvalid()
//        {
//            // Arrange
//            var guid = Guid.Empty;

//            // Act
//            var response = await _client.PutAsJsonAsync("/api/contacts", guid);

//            // Assert
//            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//        }
//    }
//}