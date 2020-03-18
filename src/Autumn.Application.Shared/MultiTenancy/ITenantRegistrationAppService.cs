using System.Threading.Tasks;
using Abp.Application.Services;
using Autumn.Editions.Dto;
using Autumn.MultiTenancy.Dto;

namespace Autumn.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}