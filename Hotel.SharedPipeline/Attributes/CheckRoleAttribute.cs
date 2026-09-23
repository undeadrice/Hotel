using Hotel.Shared.Application.Users.Enums;

namespace Hotel.SharedPipeline.Attributes;

public class CheckRoleAttribute : Attribute
{
    public UserRole[] Roles { get; set; }

    public CheckRoleAttribute(params UserRole[] roles)
    {
        Roles = roles;
    }
}
