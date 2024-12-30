using Moq;
using Contacts.Application.Services.Contacts;
using Contacts.Domain.Contacts.Dto;
using Contacts.Domain.Contacts.Models;
using Contacts.Domain.Contacts.Repositories;

namespace Contacts.Test.UnitTests;
public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _repositoryMock;
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _repositoryMock = new Mock<IContactRepository>();
        _service = new ContactService(_repositoryMock.Object);
    }

    [Fact]
    public void List_ShouldReturnAllContacts_WhenDDDIsNull()
    {
        var contacts = new List<Contact>
        {
            new() { Id = Guid.NewGuid(), Name = "Bruno", Phone = new ContactPhone { DDD = "11", Number = "988776655" }, Email = new ContactEmail { Address = "bruno@teste.com"} },
            new() { Id = Guid.NewGuid(), Name = "Lucas", Phone = new ContactPhone { DDD = "22", Number = "988776655" },Email = new ContactEmail { Address = "lucas@teste.com"} }
        };

        _repositoryMock.Setup(r => r.Query(false)).Returns(contacts.AsQueryable());

        var result = _service.List(null);

        Assert.Equal(contacts.Count, result.Count);
        _repositoryMock.Verify(r => r.Query(false), Times.Once);
    }

    [Fact]
    public void List_ShouldFilterContacts_ByDDD()
    {
        var contacts = new List<Contact>
        {
            new() { Id = Guid.NewGuid(), Name = "Bruno", Phone = new ContactPhone { DDD = "11", Number = "988776655" }, Email = new ContactEmail { Address = "bruno@teste.com"} },
            new() { Id = Guid.NewGuid(), Name = "Lucas", Phone = new ContactPhone { DDD = "22", Number = "988776655" }, Email = new ContactEmail { Address = "lucas@teste.com"} }
        };

        _repositoryMock.Setup(r => r.Query(false)).Returns(contacts.AsQueryable());

        var result = _service.List("11");

        Assert.Single(result);
        Assert.Equal("11", result.First().PhoneDDD);
    }

    [Fact]
    public void Create_ShouldCallRepositoryCreate_WhenValid()
    {
        var dto = new ContactDto { Name = "Bruno", PhoneDDD = "11", PhoneNumber = "988776655", EmailAddress = "bruno@teste.com" };

        _service.Create(dto);

        _repositoryMock.Verify(r => r.Create(It.IsAny<Contact>()), Times.Once);
    }

    [Fact]
    public void Update_ShouldCallRepositoryUpdate_WhenValid()
    {
        var contactId = Guid.NewGuid();
        var existingContact = new Contact() { Id = contactId, Name = "Bruno", Phone = new ContactPhone { DDD = "11", Number = "988776655" }, Email = new ContactEmail { Address = "bruno@teste.com" } };
        var dto = new ContactDto { Id = contactId, Name = "Raphael", PhoneDDD = "11", PhoneNumber = "988776655", EmailAddress = "raphael@teste.com" };

        _repositoryMock.Setup(r => r.GetById(contactId, false)).Returns(existingContact);

        _service.Update(dto);

        _repositoryMock.Verify(r => r.GetById(contactId, false), Times.Once);
        _repositoryMock.Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete()
    {
        var contactId = Guid.NewGuid();

        _service.Delete(contactId);

        _repositoryMock.Verify(r => r.Delete(contactId), Times.Once);
    }

    [Fact]
    public void EnsureValidation_ShouldThrowException_WhenNameIsEmpty()
    {
        var dto = new ContactDto { Name = "", PhoneDDD = "11", PhoneNumber = "988776655", EmailAddress = "bruno@teste.com" };

        var exception = Assert.Throws<ArgumentException>(() => _service.Create(dto));
        Assert.Contains("Name shouldn't be empty!", exception.Message);
    }

    [Fact]
    public void EnsureValidation_ShouldThrowException_WhenPhoneDDDIsInvalid()
    {
        var dto = new ContactDto { Name = "Bruno", PhoneDDD = "999", PhoneNumber = "988776655", EmailAddress = "bruno@teste.com" };

        var exception = Assert.Throws<ArgumentException>(() => _service.Create(dto));
        Assert.Contains("Phone DDD is invalid!", exception.Message);
    }

    [Fact]
    public void EnsureValidation_ShouldThrowException_WhenPhoneNumberIsInvalid()
    {
        var dto = new ContactDto { Name = "Bruno", PhoneDDD = "11", PhoneNumber = "123ABC789", EmailAddress = "bruno@teste.com" };

        var exception = Assert.Throws<ArgumentException>(() => _service.Create(dto));
        Assert.Contains("Phone Number should have only numbers!", exception.Message);
    }

    [Fact]
    public void EnsureValidation_ShouldThrowException_WhenEmailIsInvalid()
    {
        var dto = new ContactDto { Name = "Bruno", PhoneDDD = "11", PhoneNumber = "988776655", EmailAddress = "email-invalido" };

        var exception = Assert.Throws<ArgumentException>(() => _service.Create(dto));
        Assert.Contains("Email Address is invalid!", exception.Message);
    }
}
