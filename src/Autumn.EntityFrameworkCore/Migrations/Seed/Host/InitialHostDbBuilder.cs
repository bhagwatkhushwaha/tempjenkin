using Autumn.EntityFrameworkCore;

namespace Autumn.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly AutumnDbContext _context;

        public InitialHostDbBuilder(AutumnDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
            new DefaultCountryCreator(_context).Create();
            new DefaultRolesAndPermissions(_context).Create();
            _context.SaveChanges();
        }
    }
}
