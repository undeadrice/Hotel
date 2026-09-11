namespace Hotel.Application.Dashboard.TransferObjects;

public record DashboardDto(
    int RoomCount,
    int OccupiedRoomCount,
    int GuestCount,
    int GuestsOnSiteCount,
    double OccupancyPercentage,
    DateOnly CurrentBusinessDate);