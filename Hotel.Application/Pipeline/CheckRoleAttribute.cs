using Hotel.Application.Users.Enums;

namespace Hotel.Application.Pipeline;

public class CheckRoleAttribute : Attribute
{
    public UserRole[] Roles { get; set; }

    public CheckRoleAttribute(params UserRole[] roles)
    {
        Roles = roles;
    }
}