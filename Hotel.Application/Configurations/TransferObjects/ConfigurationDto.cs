namespace Hotel.Application.Configurations.TransferObjects;

public record ConfigurationDto(
    Guid Id,
    string TimeZoneId,
    DateOnly CurrentBusinessDate);