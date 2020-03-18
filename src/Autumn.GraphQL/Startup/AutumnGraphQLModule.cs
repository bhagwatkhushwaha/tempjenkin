using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Autumn.Startup
{
    [DependsOn(typeof(AutumnCoreModule))]
    public class AutumnGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AutumnGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}