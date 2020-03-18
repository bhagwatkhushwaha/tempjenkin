using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Autumn
{
    [DependsOn(typeof(AutumnClientModule), typeof(AbpAutoMapperModule))]
    public class AutumnXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AutumnXamarinSharedModule).GetAssembly());
        }
    }
}