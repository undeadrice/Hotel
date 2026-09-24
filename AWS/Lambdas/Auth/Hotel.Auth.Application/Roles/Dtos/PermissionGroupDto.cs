namespace Hotel.Auth.Application.Roles.Dtos;

public record PermissionGroupDto(string GroupName, IReadOnlyCollection<string> Permissions);