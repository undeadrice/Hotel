using Hotel.Application.Users.Enums;

namespace Hotel.Application.Pipeline
{
    public class CheckPermissionAttribute : Attribute
    {
        public Permission[] Permissions { get; set; }

        public CheckPermissionAttribute(params Permission[] permissions)
        {
            Permissions = permissions;
        }
    }
}