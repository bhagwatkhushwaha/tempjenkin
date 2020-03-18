using Abp.Domain.Services;

namespace Autumn
{
    public abstract class AutumnDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected AutumnDomainServiceBase()
        {
            LocalizationSourceName = AutumnConsts.LocalizationSourceName;
        }
    }
}
