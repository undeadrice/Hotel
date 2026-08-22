using FluentAssertions;
using Hotel.Domain.Guests;
using Hotel.Domain.Guests.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.Guests;

public class GuestTests
{
    private const string FirstName = "John";
    private const string LastName = "Doe";
    private const string Phone = "123456789";
    private const string Email = "john.doe@example.com";
    private const string DocumentNumber = "ABC123456";

    [Fact]
    public void Create_WithValidArguments_ShouldCreateGuest()
    {
        // Act
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Assert
        guest.Id.Should().NotBe(Guid.Empty);
        guest.FirstName.Should().Be(FirstName);
        guest.LastName.Should().Be(LastName);
        guest.Phone.Should().Be(Phone);
        guest.Email.Should().Be(Email);
        guest.DocumentNumber.Should().Be(DocumentNumber);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidFirstName_ShouldThrowGuestFirstNameRequiredException(string? firstName)
    {
        // Act
        Action act = () => Guest.Create(firstName!, LastName, Phone, Email, DocumentNumber);

        // Assert
        act.Should().Throw<GuestFirstNameRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidLastName_ShouldThrowGuestLastNameRequiredException(string? lastName)
    {
        // Act
        Action act = () => Guest.Create(FirstName, lastName!, Phone, Email, DocumentNumber);

        // Assert
        act.Should().Throw<GuestLastNameRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidPhone_ShouldThrowGuestPhoneRequiredException(string? phone)
    {
        // Act
        Action act = () => Guest.Create(FirstName, LastName, phone!, Email, DocumentNumber);

        // Assert
        act.Should().Throw<GuestPhoneRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidEmail_ShouldThrowGuestEmailRequiredException(string? email)
    {
        // Act
        Action act = () => Guest.Create(FirstName, LastName, Phone, email!, DocumentNumber);

        // Assert
        act.Should().Throw<GuestEmailRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidDocumentNumber_ShouldThrowGuestDocumentNumberRequiredException(string? documentNumber)
    {
        // Act
        Action act = () => Guest.Create(FirstName, LastName, Phone, Email, documentNumber!);

        // Assert
        act.Should().Throw<GuestDocumentNumberRequiredException>();
    }

    [Fact]
    public void UpdateProfile_WithValidArguments_ShouldUpdateProfile()
    {
        // Arrange
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Act
        guest.UpdateProfile("Jane", "Smith", "987654321", "jane.smith@example.com", "XYZ987654");

        // Assert
        guest.FirstName.Should().Be("Jane");
        guest.LastName.Should().Be("Smith");
        guest.Phone.Should().Be("987654321");
        guest.Email.Should().Be("jane.smith@example.com");
        guest.DocumentNumber.Should().Be("XYZ987654");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateProfile_WithInvalidFirstName_ShouldThrowGuestFirstNameRequiredException(string? firstName)
    {
        // Arrange
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Act
        Action act = () => guest.UpdateProfile(firstName!, LastName, Phone, Email, DocumentNumber);

        // Assert
        act.Should().Throw<GuestFirstNameRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateProfile_WithInvalidLastName_ShouldThrowGuestLastNameRequiredException(string? lastName)
    {
        // Arrange
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Act
        Action act = () => guest.UpdateProfile(FirstName, lastName!, Phone, Email, DocumentNumber);

        // Assert
        act.Should().Throw<GuestLastNameRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateProfile_WithInvalidPhone_ShouldThrowGuestPhoneRequiredException(string? phone)
    {
        // Arrange
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Act
        Action act = () => guest.UpdateProfile(FirstName, LastName, phone!, Email, DocumentNumber);

        // Assert
        act.Should().Throw<GuestPhoneRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateProfile_WithInvalidEmail_ShouldThrowGuestEmailRequiredException(string? email)
    {
        // Arrange
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Act
        Action act = () => guest.UpdateProfile(FirstName, LastName, Phone, email!, DocumentNumber);

        // Assert
        act.Should().Throw<GuestEmailRequiredException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateProfile_WithInvalidDocumentNumber_ShouldThrowGuestDocumentNumberRequiredException(string? documentNumber)
    {
        // Arrange
        var guest = Guest.Create(FirstName, LastName, Phone, Email, DocumentNumber);

        // Act
        Action act = () => guest.UpdateProfile(FirstName, LastName, Phone, Email, documentNumber!);

        // Assert
        act.Should().Throw<GuestDocumentNumberRequiredException>();
    }
}