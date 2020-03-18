using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Autumn.Authorization.Permissions.Dto;

namespace Autumn.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
