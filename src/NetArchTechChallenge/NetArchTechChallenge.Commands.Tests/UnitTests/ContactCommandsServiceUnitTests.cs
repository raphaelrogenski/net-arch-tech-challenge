using Moq;
using NetArchTechChallenge.Shared.Application.DTOs;
using NetArchTechChallenge.Shared.Application.Messaging;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Application.Validators;
using NetArchTechChallenge.Shared.Domain.Events;

namespace NetArchTechChallenge.Commands.Tests.UnitTests
{
    public class ContactCommandsServiceUnitTests
    {
        private readonly Mock<IMessageService> _messageServiceMock;
        private readonly Mock<IContactUniquenessValidator> _uniquenessValidatorMock;
        private readonly ContactCommandsService _service;

        public ContactCommandsServiceUnitTests()
        {
            _messageServiceMock = new();
            _uniquenessValidatorMock = new();
            _service = new ContactCommandsService(_messageServiceMock.Object, _uniquenessValidatorMock.Object);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenNameIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Name shouldn't be empty!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenPhoneDDDIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone DDD is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenPhoneDDDIsInvalid()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "00",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone DDD is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenPhoneNumberIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone Number is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "333333333",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone Number is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenEmailAddressIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Email Address is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenEmailAddressIsInvalid()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test.email.com",
            };

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Email Address is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenNameIsInUse()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            _uniquenessValidatorMock
                .Setup(x => x.ContactNameAlreadyInUse(request.Name, It.IsAny<Guid>()))
                .ReturnsAsync(true);

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Name already in use!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenPhoneIsInUse()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            _uniquenessValidatorMock
                .Setup(x => x.ContactPhoneAlreadyInUse(request.PhoneDDD, request.PhoneNumber, It.IsAny<Guid>()))
                .ReturnsAsync(true);

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone already in use!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Create_ShouldFail_WhenEmailIsInUse()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            _uniquenessValidatorMock
                .Setup(x => x.ContactEmailAlreadyInUse(request.EmailAddress, It.IsAny<Guid>()))
                .ReturnsAsync(true);

            try
            {
                // Act
                _service.Create(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Email already in use!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Never);
        }

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

            _uniquenessValidatorMock
                .Setup(x => x.ContactNameAlreadyInUse(request.Name, It.IsAny<Guid>()))
                .ReturnsAsync(false);

            _uniquenessValidatorMock
                .Setup(x => x.ContactPhoneAlreadyInUse(request.PhoneDDD, request.PhoneNumber, It.IsAny<Guid>()))
                .ReturnsAsync(false);

            _uniquenessValidatorMock
                .Setup(x => x.ContactEmailAlreadyInUse(request.EmailAddress, It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            _service.Create(request);

            // Assert
            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactCreatedEvent>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenNameIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Name shouldn't be empty!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenPhoneDDDIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone DDD is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenPhoneDDDIsInvalid()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "00",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone DDD is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenPhoneNumberIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone Number is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "333333333",
                EmailAddress = "test@email.com",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone Number is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenEmailAddressIsEmpty()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Email Address is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenEmailAddressIsInvalid()
        {
            // Arrange
            var request = new ContactDto
            {
                Id = Guid.NewGuid(),
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test.email.com",
            };

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Email Address is invalid!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenNameIsInUse()
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

            _uniquenessValidatorMock
                .Setup(x => x.ContactNameAlreadyInUse(request.Name, It.IsAny<Guid>()))
                .ReturnsAsync(true);

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Name already in use!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenPhoneIsInUse()
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

            _uniquenessValidatorMock
                .Setup(x => x.ContactPhoneAlreadyInUse(request.PhoneDDD, request.PhoneNumber, It.IsAny<Guid>()))
                .ReturnsAsync(true);

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Phone already in use!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldFail_WhenEmailIsInUse()
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

            _uniquenessValidatorMock
                .Setup(x => x.ContactEmailAlreadyInUse(request.EmailAddress, It.IsAny<Guid>()))
                .ReturnsAsync(true);

            try
            {
                // Act
                _service.Update(request);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsType<Shared.Application.Exceptions.ApplicationException>(ex);
                Assert.Same("Email already in use!", ex.Message);
            }

            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldSucceed_WhenDataIsValid()
        {
            // Arrange
            var request = new ContactDto
            {
                Name = "Raphael",
                PhoneDDD = "42",
                PhoneNumber = "999999999",
                EmailAddress = "test@email.com",
            };

            _uniquenessValidatorMock
                .Setup(x => x.ContactNameAlreadyInUse(request.Name, It.IsAny<Guid>()))
                .ReturnsAsync(false);

            _uniquenessValidatorMock
                .Setup(x => x.ContactPhoneAlreadyInUse(request.PhoneDDD, request.PhoneNumber, It.IsAny<Guid>()))
                .ReturnsAsync(false);

            _uniquenessValidatorMock
                .Setup(x => x.ContactEmailAlreadyInUse(request.EmailAddress, It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            _service.Update(request);

            // Assert
            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactUpdatedEvent>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldSucceed_WhenDataIsValid()
        {
            // Arrange
            var guid = Guid.NewGuid();

            // Act
            _service.Delete(guid);

            // Assert
            _messageServiceMock.Verify(x => x.Publish(It.IsAny<ContactDeletedEvent>()), Times.Once);
        }
    }
}