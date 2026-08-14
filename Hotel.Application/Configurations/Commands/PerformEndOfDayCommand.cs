using Hotel.Application.Pipeline;
using Hotel.Application.Users.Enums;

namespace Hotel.Application.Configurations.Commands;

[CheckPermission(Permission.ConfigurationEdit)]
public record PerformEndOfDayCommand : ICommand<DateOnly>;