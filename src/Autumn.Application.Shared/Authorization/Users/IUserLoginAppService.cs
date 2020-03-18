using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Autumn.Authorization.Users.Dto;

namespace Autumn.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<ListResultDto<UserLoginAttemptDto>> GetRecentUserLoginAttempts();
    }
}
