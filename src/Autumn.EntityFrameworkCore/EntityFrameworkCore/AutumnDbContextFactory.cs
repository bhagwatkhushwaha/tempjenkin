using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Autumn.Configuration;
using Autumn.Web;

namespace Autumn.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class AutumnDbContextFactory : IDesignTimeDbContextFactory<AutumnDbContext>
    {
        public AutumnDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AutumnDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder(), addUserSecrets: true);

            AutumnDbContextConfigurer.Configure(builder, configuration.GetConnectionString(AutumnConsts.ConnectionStringName));

            return new AutumnDbContext(builder.Options);
        }
    }
}