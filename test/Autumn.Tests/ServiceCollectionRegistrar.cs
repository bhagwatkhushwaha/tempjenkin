//using Abp.Dependency;
//using Castle.MicroKernel.Registration;
//using Castle.Windsor.MsDependencyInjection;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Autumn.EntityFrameworkCore;
//using Autumn.Identity;

//namespace Autumn.Tests
//{
//    public static class ServiceCollectionRegistrar
//    {
//        public static void Register(IIocManager iocManager)
//        {
//            RegisterIdentity(iocManager);

//            var builder = new DbContextOptionsBuilder<AutumnDbContext>();

//            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
//            builder.UseSqlite(inMemorySqlite);

//            iocManager.IocContainer.Register(
//                Component
//                    .For<DbContextOptions<AutumnDbContext>>()
//                    .Instance(builder.Options)
//                    .LifestyleSingleton()
//            );

//            inMemorySqlite.Open();

//            new AutumnDbContext(builder.Options).Database.EnsureCreated();
//        }

//        private static void RegisterIdentity(IIocManager iocManager)
//        {
//            var services = new ServiceCollection();

//            IdentityRegistrar.Register(services);

//            WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);
//        }
//    }
//}
