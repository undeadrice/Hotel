using Hotel.Shared.Application.Users.Enums;

namespace Hotel.SharedPipeline.Attributes
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
