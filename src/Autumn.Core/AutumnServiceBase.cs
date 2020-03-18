using Abp;

namespace Autumn
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="AutumnDomainServiceBase"/>.
    /// For application services inherit AutumnAppServiceBase.
    /// </summary>
    public abstract class AutumnServiceBase : AbpServiceBase
    {
        protected AutumnServiceBase()
        {
            LocalizationSourceName = AutumnConsts.LocalizationSourceName;
        }
    }
}