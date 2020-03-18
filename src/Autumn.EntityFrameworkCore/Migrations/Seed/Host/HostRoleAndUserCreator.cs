using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.EntityFrameworkCore;
using Autumn.Authorization;
using Autumn.Authorization.Roles;
using Autumn.Authorization.Users;
using Autumn.EntityFrameworkCore;
using Autumn.Notifications;

namespace Autumn.Migrations.Seed.Host
{
    public class HostRoleAndUserCreator
    {
        private readonly AutumnDbContext _context;

        public HostRoleAndUserCreator(AutumnDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateHostRoleAndUsers();
        }

        private void CreateHostRoleAndUsers()
        {
            //Admin role for host

            var adminRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Admin);
            if (adminRoleForHost == null)
            {
                adminRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Admin, StaticRoleNames.Host.Admin) { IsStatic = true, IsDefault = true }).Entity;
                _context.SaveChanges();
            }
            
            //User role for host

            var userRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.User);
            if (userRoleForHost == null)
            {
                userRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.User, StaticRoleNames.Host.User) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //Guest role for host

            var guestRoleForHost = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == StaticRoleNames.Host.Guest);
            if (guestRoleForHost == null)
            {
                guestRoleForHost = _context.Roles.Add(new Role(null, StaticRoleNames.Host.Guest, StaticRoleNames.Host.Guest) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //admin user for host

            var adminUserForHost = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == null && u.UserName == AbpUserBase.AdminUserName);
            if (adminUserForHost == null)
            {
                var user = new User
                {
                    TenantId = null,
                    UserName = AbpUserBase.AdminUserName,
                    Name = "admin",
                    Surname = "admin",
                    EmailAddress = "admin@aspnetzero.com",
                    IsEmailConfirmed = true,
                    ShouldChangePasswordOnNextLogin = false,
                    IsActive = true,
                    Password = "AM4OLBpptxBYmM79lGOX9egzZk3vIQU3d/gFCJzaBjAPXzYIK3tQ2N7X4fcrHtElTw==" //123qwe
                };

                user.SetNormalizedNames();

                adminUserForHost = _context.Users.Add(user).Entity;
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(null, adminUserForHost.Id, adminRoleForHost.Id));
                _context.SaveChanges();

                //User account of admin user
                _context.UserAccounts.Add(new UserAccount
                {
                    TenantId = null,
                    UserId = adminUserForHost.Id,
                    UserName = AbpUserBase.AdminUserName,
                    EmailAddress = adminUserForHost.EmailAddress
                });

                _context.SaveChanges();

                //Notification subscriptions
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), null, adminUserForHost.Id, AppNotificationNames.NewTenantRegistered));
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), null, adminUserForHost.Id, AppNotificationNames.NewUserRegistered));

                _context.SaveChanges();
            }
        }
    }
}