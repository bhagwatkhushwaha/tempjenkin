using System.Threading.Tasks;
using Abp.Application.Services;
using Autumn.Configuration.Tenants.Dto;

namespace Autumn.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
