using System.Threading.Tasks;
using Abp.Application.Services;
using Autumn.Install.Dto;

namespace Autumn.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}