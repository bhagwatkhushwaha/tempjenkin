using Abp.AspNetCore.Mvc.Views;

namespace Autumn.Web.Views
{
    public abstract class AutumnRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected AutumnRazorPage()
        {
            LocalizationSourceName = AutumnConsts.LocalizationSourceName;
        }
    }
}
