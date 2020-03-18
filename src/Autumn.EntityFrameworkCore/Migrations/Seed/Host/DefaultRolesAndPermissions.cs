using Autumn.Authorization.Roles;
using Autumn.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autumn.Migrations.Seed.Host
{
    public class DefaultRolesAndPermissions
    {
        private readonly AutumnDbContext _context;

        public DefaultRolesAndPermissions(AutumnDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateDefaultRolesAndPermissions();
        }

        private void CreateDefaultRolesAndPermissions()
        {
            var GoalsOnly = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "GoalsOnly");
            if (GoalsOnly == null)
            {
                GoalsOnly = _context.Roles.Add(new Role(null, "GoalsOnly", "Goals-Only") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }


            var LimitedAccess = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "LimitedAccess");
            if (LimitedAccess == null)
            {
                LimitedAccess = _context.Roles.Add(new Role(null, "LimitedAccess", "Limited access") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }


            var FullAccess = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "FullAccess");
            if (FullAccess == null)
            {
                FullAccess = _context.Roles.Add(new Role(null, "FullAccess", "Full Access") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }


            var Insurance = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "Insurance");
            if (Insurance == null)
            {
                Insurance = _context.Roles.Add(new Role(null, "Insurance", "Insurance-only") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }


            var Investments = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "Investments");
            if (Investments == null)
            {
                Investments = _context.Roles.Add(new Role(null, "Investments", "Investments-only") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }

            var WealthPlanner = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "WealthPlanner");
            if (WealthPlanner == null)
            {
                WealthPlanner = _context.Roles.Add(new Role(null, "WealthPlanner", "Wealth planner") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }

            var FullFinancialAdvice = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == null && r.Name == "FullFinancialAdvice");
            if (FullFinancialAdvice == null)
            {
                FullFinancialAdvice = _context.Roles.Add(new Role(null, "FullFinancialAdvice", "Full financial advice") { IsStatic = true, IsDefault = false }).Entity;
                _context.SaveChanges();
            }

        }
    }
}
