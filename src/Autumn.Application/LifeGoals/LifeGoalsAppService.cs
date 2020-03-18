using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Autumn.Goals;
using Autumn.Goals.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.LifeGoals
{
    [AbpAuthorize]
    public class LifeGoalsAppService : AutumnAppServiceBase, ILifeGoalsAppService
    {
        private readonly IRepository<Goals.LifeGoals> _lifeGoalsRepo;

        public LifeGoalsAppService(IRepository<Goals.LifeGoals> lifeGoalsRepo)
        {
            _lifeGoalsRepo = lifeGoalsRepo;
        }

        public async Task CreateOrUpdateGoal(LifeGoalsDto input)
        {
            try
            {
                if(input.Id != 0)
                {
                    await UpdateGoal(input);
                }
                else
                {
                    await CreateGoal(input);
                }
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task CreateGoal(LifeGoalsDto input)
        {
            try
            {
                var goal = ObjectMapper.Map<Goals.LifeGoals>(input);
                await _lifeGoalsRepo.InsertAsync(goal);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private async Task UpdateGoal(LifeGoalsDto input)
        {
            try
            {
                var goal = await _lifeGoalsRepo.FirstOrDefaultAsync((int)input.Id);
                ObjectMapper.Map(input, goal);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task DeleteGoal(EntityDto input)
        {
            try
            {
                await _lifeGoalsRepo.DeleteAsync(input.Id);
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<LifeGoalsDto>> GetGoals()
        {
            try
            {
                var goals = await _lifeGoalsRepo.GetAll().Where(x => x.CreatorUserId == AbpSession.UserId).OrderBy(x=>x.CreationTime).ToListAsync();
                
                return ObjectMapper.Map<List<LifeGoalsDto>>(goals);
                
            }
            catch (UserFriendlyException e)
            {
                throw new UserFriendlyException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
