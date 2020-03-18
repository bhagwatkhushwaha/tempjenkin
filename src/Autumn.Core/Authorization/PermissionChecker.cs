using Abp.Authorization;
using Autumn.Authorization.Roles;
using Autumn.Authorization.Users;

namespace Autumn.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
