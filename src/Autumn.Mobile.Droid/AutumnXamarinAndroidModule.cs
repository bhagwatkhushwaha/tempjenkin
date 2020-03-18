using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Autumn
{
    [DependsOn(typeof(AutumnXamarinSharedModule))]
    public class AutumnXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AutumnXamarinAndroidModule).GetAssembly());
        }
    }
}