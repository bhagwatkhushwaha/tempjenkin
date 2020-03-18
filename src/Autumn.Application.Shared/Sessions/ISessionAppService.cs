using System.Threading.Tasks;
using Abp.Application.Services;
using Autumn.Sessions.Dto;

namespace Autumn.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
