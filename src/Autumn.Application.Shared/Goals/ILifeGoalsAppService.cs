using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Autumn.Goals.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.Goals
{
    public interface ILifeGoalsAppService: IApplicationService
    {
        Task CreateOrUpdateGoal(LifeGoalsDto goal);

        Task DeleteGoal(EntityDto goal);

        Task<List<LifeGoalsDto>> GetGoals();
    }
}
