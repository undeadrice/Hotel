using FluentAssertions;
using Hotel.Domain.NumberCycles.Entities;
using Hotel.Domain.NumberCycles.Enums;
using Hotel.Domain.NumberCycles.Exceptions;
using Xunit;

namespace Hotel.Domain.Tests.NumberCycles.Entities;

public class NumberCycleTests
{
    [Fact]
    public void Create_WithValidArguments_ShouldSetProperties()
    {
        // Act
        var cycle = NumberCycle.Create(NumberCycleTopic.Reservation, " res ", 10);

        // Assert
        cycle.Id.Should().NotBe(Guid.Empty);
        cycle.Topic.Should().Be(NumberCycleTopic.Reservation);
        cycle.Prefix.Should().Be("RES");
        cycle.StartIndex.Should().Be(10);
        cycle.CurrentIndex.Should().Be(10);
        cycle.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_WhenTopicNotDefined_ShouldThrowNumberCycleInvalidTopicException()
    {
        // Act
        Action act = () => NumberCycle.Create((NumberCycleTopic)999, "RES", 0);

        // Assert
        act.Should().Throw<NumberCycleInvalidTopicException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WhenPrefixInvalid_ShouldThrowNumberCyclePrefixRequiredException(string? prefix)
    {
        // Act
        Action act = () => NumberCycle.Create(NumberCycleTopic.Reservation, prefix!, 0);

        // Assert
        act.Should().Throw<NumberCyclePrefixRequiredException>();
    }

    [Fact]
    public void Create_WhenStartIndexNegative_ShouldThrowNumberCycleStartIndexInvalidException()
    {
        // Act
        Action act = () => NumberCycle.Create(NumberCycleTopic.Reservation, "RES", -1);

        // Assert
        act.Should().Throw<NumberCycleStartIndexInvalidException>();
    }

    [Fact]
    public void NextIdentifier_ShouldReturnFormattedIdentifierAndIncrementCurrentIndex()
    {
        // Arrange
        var cycle = NumberCycle.Create(NumberCycleTopic.FiscalAccount, "FA", 5);

        // Act
        var first = cycle.NextIdentifier();
        var second = cycle.NextIdentifier();

        // Assert
        first.Should().Be("FA-5");
        second.Should().Be("FA-6");
        cycle.CurrentIndex.Should().Be(7);
    }
}