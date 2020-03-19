using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Autumn.Configure;
using Autumn.Startup;
using Autumn.Test.Base;

namespace Autumn.GraphQL.Tests
{
    [DependsOn(
        typeof(AutumnGraphQLModule),
        typeof(AutumnTestBaseModule))]
    public class AutumnGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AutumnGraphQLTestModule).GetAssembly());
        }
    }
}