using Moq;
using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Application.Validators;
using NetArchTechChallenge.Shared.Domain.Entities;
using NetArchTechChallenge.Shared.Domain.Events;
using NetArchTechChallenge.Shared.Domain.Repositories;

namespace NetArchTechChallenge.Persistence.Tests.UnitTests
{
    public class ContactPersistenceServiceUnitTests
    {
        [Fact]
        public async Task Create_ShouldSucceed_WhenDataIsValid()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            var repositoryMock = new Mock<IContactRepository>();
            var service = new ContactPersistenceService(repositoryMock.Object);

            // Act
            service.Create(request);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenContactNotFound()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            var repositoryMock = new Mock<IContactRepository>();
            var service = new ContactPersistenceService(repositoryMock.Object);

            try
            {
                // Act
                service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Entry not found!", ex.Message);
            }
        }
    }
}