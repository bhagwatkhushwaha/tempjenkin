using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Autumn
{
    [DependsOn(typeof(AutumnXamarinSharedModule))]
    public class AutumnXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AutumnXamarinIosModule).GetAssembly());
        }
    }
}