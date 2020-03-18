﻿using System.Threading.Tasks;
using Abp.Application.Services;
using Autumn.Configuration.Host.Dto;

namespace Autumn.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
