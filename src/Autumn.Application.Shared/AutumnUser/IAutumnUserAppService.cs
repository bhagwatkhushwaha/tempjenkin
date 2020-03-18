using Abp.Application.Services;
using Autumn.AutumnUser.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.AutumnUser
{
    public interface IAutumnUserAppService : IApplicationService
    {
        Task CreateUser(AutumnUserDto user);

        Task<AutumnUserDto> CheckUserProgress();

        Task UpdateUser(AutumnUserDto user);

        Task<bool> CheckEmailExists(string email);

        Task<GraphDataDto> GenGraphData(AutumnUserDto userInfo);

    }
}
