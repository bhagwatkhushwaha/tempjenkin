using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Autumn
{
    public class AutumnClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(AutumnClientModule).GetAssembly());
        }
    }
}
