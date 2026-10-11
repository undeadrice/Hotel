using Hotel.Domain.NumberCycles.Enums;

namespace Hotel.Application.NumberCycles.TransferObjects;

public record NumberCycleDto(
    Guid Id,
    NumberCycleTopic Topic,
    string Prefix,
    int StartIndex,
    int CurrentIndex,
    DateTime CreatedAt);