using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Autumn.EntityFrameworkCore
{
    public static class AutumnDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<AutumnDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<AutumnDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}