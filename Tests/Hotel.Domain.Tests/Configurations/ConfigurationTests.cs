using FluentAssertions;
using Hotel.Domain.Configurations.Entities;
using Xunit;

namespace Hotel.Domain.Tests.Configurations;

public class ConfigurationTests
{
    private const string TimeZoneId = "Central European Standard Time";
    private static readonly DateOnly BusinessDate = new(2026, 8, 22);

    [Fact]
    public void Create_WithValidArguments_ShouldCreateConfiguration()
    {
        // Act
        var configuration = Configuration.Create(TimeZoneId, BusinessDate);

        // Assert
        configuration.Id.Should().NotBe(Guid.Empty);
        configuration.TimeZone.Id.Should().Be(TimeZoneId);
        configuration.CurrentBusinessDate.Should().Be(BusinessDate);
    }

    [Fact]
    public void Create_WithInvalidTimeZoneId_ShouldThrowTimeZoneNotFoundException()
    {
        // Act
        Action act = () => Configuration.Create("Invalid/TimeZone", BusinessDate);

        // Assert
        act.Should().Throw<TimeZoneNotFoundException>();
    }

    [Fact]
    public void Update_WithValidArguments_ShouldUpdateConfiguration()
    {
        // Arrange
        var configuration = Configuration.Create(TimeZoneId, BusinessDate);
        var newTimeZoneId = "UTC";
        var newBusinessDate = new DateOnly(2026, 12, 31);

        // Act
        configuration.Update(newTimeZoneId, newBusinessDate);

        // Assert
        configuration.TimeZone.Id.Should().Be(newTimeZoneId);
        configuration.CurrentBusinessDate.Should().Be(newBusinessDate);
    }

    [Fact]
    public void Update_WithInvalidTimeZoneId_ShouldThrowTimeZoneNotFoundException()
    {
        // Arrange
        var configuration = Configuration.Create(TimeZoneId, BusinessDate);

        // Act
        Action act = () => configuration.Update("Invalid/TimeZone", BusinessDate);

        // Assert
        act.Should().Throw<TimeZoneNotFoundException>();
    }

    [Fact]
    public void EndOfDay_ShouldAdvanceCurrentBusinessDateByOneDay()
    {
        // Arrange
        var configuration = Configuration.Create(TimeZoneId, BusinessDate);

        // Act
        configuration.EndOfDay();

        // Assert
        configuration.CurrentBusinessDate.Should().Be(BusinessDate.AddDays(1));
    }

    [Fact]
    public void EndOfDay_ShouldAdvanceAcrossMonthBoundary()
    {
        // Arrange
        var configuration = Configuration.Create(TimeZoneId, new DateOnly(2026, 1, 31));

        // Act
        configuration.EndOfDay();

        // Assert
        configuration.CurrentBusinessDate.Should().Be(new DateOnly(2026, 2, 1));
    }
}