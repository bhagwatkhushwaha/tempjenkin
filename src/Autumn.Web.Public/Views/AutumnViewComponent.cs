using Abp.AspNetCore.Mvc.ViewComponents;

namespace Autumn.Web.Public.Views
{
    public abstract class AutumnViewComponent : AbpViewComponent
    {
        protected AutumnViewComponent()
        {
            LocalizationSourceName = AutumnConsts.LocalizationSourceName;
        }
    }
}