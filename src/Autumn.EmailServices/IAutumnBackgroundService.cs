using Abp.BackgroundJobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.EmailServices
{
    public interface IAutumnBackgroundService
    {
        Task<string> ScheduleJob<TJob, TArgs>(TArgs args, DateTimeOffset timeOffset) where TJob : IBackgroundJob<TArgs>;
        Task<string> EnqueueJob<TJob, TArgs>(TArgs args) where TJob : IBackgroundJob<TArgs>;
        void DeleteJob(string jobId);
        Task<int> AddOrUpdateAsync<TJob, TArgs>(string recurringJobId, TArgs args, string cronExpressions, string timeZoneId, BackgroundJobPriority priority = BackgroundJobPriority.Normal) where TJob : IBackgroundJob<TArgs>;
    }
}
