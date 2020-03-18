using Abp.Application.Services;
using Autumn.Dto;
using Autumn.Logging.Dto;

namespace Autumn.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
