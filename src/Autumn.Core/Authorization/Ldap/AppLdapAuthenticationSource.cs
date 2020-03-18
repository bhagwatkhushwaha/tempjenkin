using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Autumn.Authorization.Users;
using Autumn.MultiTenancy;

namespace Autumn.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}