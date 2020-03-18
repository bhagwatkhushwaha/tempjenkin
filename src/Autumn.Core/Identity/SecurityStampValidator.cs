using Abp.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Autumn.Authorization.Roles;
using Autumn.Authorization.Users;
using Autumn.MultiTenancy;

namespace Autumn.Identity
{
    public class SecurityStampValidator : AbpSecurityStampValidator<Tenant, Role, User>
    {
        public SecurityStampValidator(
            IOptions<SecurityStampValidatorOptions> options, 
            SignInManager signInManager,
            ISystemClock systemClock)
            : base(options, signInManager, systemClock)
        {
        }
    }
}