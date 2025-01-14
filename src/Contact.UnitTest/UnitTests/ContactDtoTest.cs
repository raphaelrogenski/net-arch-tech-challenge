using Contacts.Domain.Contacts.Dto;
using Contacts.Domain.Contacts.Models;
using FluentAssertions;

namespace Contacts.UnitTest.UnitTests;
public class ContactDtoTests
{
    [Fact]
    public void Cast_EntityToDto_ShouldMapPropertiesCorrectly()
    {
        var contact = new Contact
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            Name = "Bruno",
            Phone = new ContactPhone
            {
                DDD = "11",
                Number = "987654321"
            },
            Email = new ContactEmail
            {
                Address = "bruno@teste.com"
            }
        };

        var result = ContactDto.Cast(contact);

        result.Id.Should().Be(contact.Id);
        result.CreatedAt.Should().Be(contact.CreatedAt);
        result.Name.Should().Be(contact.Name);
        result.PhoneDDD.Should().Be(contact.Phone.DDD);
        result.PhoneNumber.Should().Be(contact.Phone.Number);
        result.EmailAddress.Should().Be(contact.Email.Address);
    }

    [Fact]
    public void Cast_DtoToEntity_NewEntity_ShouldCreateNewEntity()
    {
        var dto = new ContactDto
        {
            Id = Guid.Empty,
            Name = "Lucas",
            PhoneDDD = "21",
            PhoneNumber = "123456789",
            EmailAddress = "lucas@teste.com"
        };

        var result = ContactDto.Cast(dto);

        result.Name.Should().Be(dto.Name);
        result.Phone.DDD.Should().Be(dto.PhoneDDD);
        result.Phone.Number.Should().Be(dto.PhoneNumber);
        result.Email.Address.Should().Be(dto.EmailAddress);
    }

    [Fact]
    public void Cast_DtoToEntity_ExistingEntity_ShouldUpdateEntity()
    {
        var existingEntity = new Contact
        {
            Id = Guid.NewGuid(),
            Name = "Bruno",
            Phone = new ContactPhone
            {
                DDD = "11",
                Number = "987654321"
            },
            Email = new ContactEmail
            {
                Address = "bruno@teste.com"
            }
        };

        var dto = new ContactDto
        {
            Id = existingEntity.Id,
            Name = "Raphael",
            PhoneDDD = "21",
            PhoneNumber = "123456789",
            EmailAddress = "raphael@teste.com"
        };

        var result = ContactDto.Cast(dto, existingEntity);

        result.Should().Be(existingEntity);
        result.Name.Should().Be(dto.Name);
        result.Phone.DDD.Should().Be(dto.PhoneDDD);
        result.Phone.Number.Should().Be(dto.PhoneNumber);
        result.Email.Address.Should().Be(dto.EmailAddress);
    }

    [Fact]
    public void Cast_DtoToEntity_NoChanges_ShouldThrowArgumentException()
    {
        var existingEntity = new Contact
        {
            Id = Guid.NewGuid(),
            Name = "Bruno",
            Phone = new ContactPhone
            {
                DDD = "11",
                Number = "987654321"
            },
            Email = new ContactEmail
            {
                Address = "bruno@teste.com"
            }
        };

        var dto = new ContactDto
        {
            Id = existingEntity.Id,
            Name = "Bruno",
            PhoneDDD = "11",
            PhoneNumber = "987654321",
            EmailAddress = "bruno@teste.com"
        };

        Action act = () => ContactDto.Cast(dto, existingEntity);

        act.Should().Throw<ArgumentException>().WithMessage("Nothing to update!");
    }

    [Fact]
    public void Cast_DtoToEntity_NullEntity_ShouldThrowArgumentException()
    {
        var dto = new ContactDto
        {
            Id = Guid.NewGuid(),
            Name = "Lucas",
            PhoneDDD = "21",
            PhoneNumber = "123456789",
            EmailAddress = "lucas@teste.com"
        };

        Action act = () => ContactDto.Cast(dto, null);

        act.Should().Throw<ArgumentException>().WithMessage("Entry not found!");
    }
}