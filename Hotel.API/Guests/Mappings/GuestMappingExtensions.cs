using Hotel.API.Guests.Responses;
using Hotel.Application.Guests.TransferObjects;

namespace Hotel.API.Guests.Mappings;

public static class GuestMappingExtensions
{
    public static GuestResponse MapToGuestResponse(this GuestDto dto) =>
        new GuestResponse(
            dto.Id,
            dto.FirstName,
            dto.LastName,
            dto.Phone,
            dto.Email,
            dto.DocumentNumber);

    public static GuestListResponse MapToGuestListResponse(this GuestListDto dto) =>
        new GuestListResponse(
            dto.Id,
            dto.FullName,
            dto.Phone,
            dto.Email,
            dto.DocumentNumber);
}