using Moq;
using NetArchTechChallenge.Shared.Application.Services;
using NetArchTechChallenge.Shared.Domain.Entities;
using NetArchTechChallenge.Shared.Domain.Repositories;

namespace NetArchTechChallenge.Query.Tests.UnitTests
{
    public class ContactQueryServiceUnitTests
    {
        private readonly Mock<IContactRepository> _repoMock;
        private readonly ContactQueryService _service;

        public ContactQueryServiceUnitTests()
        {
            _repoMock = new Mock<IContactRepository>();
            _service = new ContactQueryService(_repoMock.Object);
        }

        [Fact]
        public void List_ShouldReturnAll_WhenDddIsEmpty()
        {
            // Arrange
            var contact1 = new Contact() { Id = Guid.NewGuid(), Name = "Raphael", PhoneDDD = "41", PhoneNumber = "999999999", EmailAddress = "test1@email.com" };
            var contact2 = new Contact() { Id = Guid.NewGuid(), Name = "Rogenski", PhoneDDD = "42", PhoneNumber = "999999998", EmailAddress = "test2@email.com" };

            var contacts = new List<Contact>
            {
                contact1,
                contact2
            };

            _repoMock.Setup(r => r.Query(false)).Returns(contacts.AsQueryable());

            // Act
            var result = _service.List(null);
            var item1 = result.SingleOrDefault(r => r.Name == contact1.Name);
            var item2 = result.SingleOrDefault(r => r.Name == contact2.Name);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.NotNull(item1);
            Assert.NotNull(item2);
            Assert.Equal(contact1.EmailAddress, item1.EmailAddress);
            Assert.Equal(contact2.EmailAddress, item2.EmailAddress);
        }

        [Fact]
        public void List_ShouldFilterByDDD()
        {
            // Arrange
            var contact1 = new Contact() { Id = Guid.NewGuid(), Name = "Raphael", PhoneDDD = "41", PhoneNumber = "999999999", EmailAddress = "test1@email.com" };
            var contact2 = new Contact() { Id = Guid.NewGuid(), Name = "Rogenski", PhoneDDD = "42", PhoneNumber = "999999998", EmailAddress = "test2@email.com" };

            var contacts = new List<Contact>
            {
                contact1,
                contact2,
            };

            _repoMock.Setup(r => r.Query(false)).Returns(contacts.AsQueryable());

            // Act
            var result = _service.List("41");
            var item1 = result.SingleOrDefault(r => r.Name == contact1.Name);

            // Assert
            Assert.Equal(1, result.Count);
            Assert.NotNull(item1);
            Assert.Equal(contact1.EmailAddress, item1.EmailAddress);
        }

        [Fact]
        public void ContactNameAlreadyExists_ShouldReturnTrue_WhenRepoSaysTrue()
        {
            // Arrange
            _repoMock.Setup(r => r.ContactNameAlreadyExists("Raphael", It.IsAny<Guid>()))
                .Returns(true);

            // Act
            var exists = _service.ContactNameAlreadyExists("Raphael");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void ContactNameAlreadyExists_ShouldReturnFalse_WhenRepoSaysFalse()
        {
            // Arrange
            _repoMock.Setup(r => r.ContactNameAlreadyExists("Raphael", It.IsAny<Guid>()))
                .Returns(false);

            // Act
            var exists = _service.ContactNameAlreadyExists("Raphael");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void ContactPhoneAlreadyExists_ShouldReturnTrue_WhenRepoSaysTrue()
        {
            // Arrange
            _repoMock.Setup(r => r.ContactPhoneAlreadyExists("41", "999999999", It.IsAny<Guid>()))
                .Returns(true);

            // Act
            var exists = _service.ContactPhoneAlreadyExists("41", "999999999");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void ContactPhoneAlreadyExists_ShouldReturnFalse_WhenRepoSaysFalse()
        {
            // Arrange
            _repoMock.Setup(r => r.ContactPhoneAlreadyExists("41", "999999999", It.IsAny<Guid>()))
                .Returns(false);

            // Act
            var exists = _service.ContactPhoneAlreadyExists("41", "999999999");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public void ContactEmailAlreadyExists_ShouldReturnTrue_WhenRepoSaysTrue()
        {
            // Arrange
            _repoMock.Setup(r => r.ContactEmailAlreadyExists("test1@email.com", It.IsAny<Guid>()))
                .Returns(true);

            // Act
            var exists = _service.ContactEmailAlreadyExists("test1@email.com");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public void ContactEmailAlreadyExists_ShouldReturnFalse_WhenRepoSaysFalse()
        {
            // Arrange
            _repoMock.Setup(r => r.ContactEmailAlreadyExists("test1@email.com", It.IsAny<Guid>()))
                .Returns(false);

            // Act
            var exists = _service.ContactEmailAlreadyExists("test1@email.com");

            // Assert
            Assert.False(exists);
        }
    }
}