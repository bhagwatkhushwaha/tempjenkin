using Abp.BackgroundJobs;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Autumn.EmailServices
{
    public class HangfireService : IAutumnBackgroundService
    {
        public Task<int> AddOrUpdateAsync<TJob, TArgs>(string recurringJobId, TArgs args, string cronExpressions, string timeZoneId, BackgroundJobPriority priority = BackgroundJobPriority.Normal) where TJob : IBackgroundJob<TArgs>
        {
            RecurringJob.AddOrUpdate<TJob>(recurringJobId, job => job.Execute(args), cronExpressions, TimeZoneInfo.FindSystemTimeZoneById(timeZoneId));
            return Task.FromResult(0);
        }

        public void DeleteJob(string args)
        {
            BackgroundJob.Delete(args);
        }

        public async Task<string> EnqueueJob<TJob, TArgs>(TArgs args) where TJob : IBackgroundJob<TArgs>
        {
            return BackgroundJob.Enqueue<TJob>((x) => x.Execute(args));
        }

        public async Task<string> ScheduleJob<TJob, TArgs>(TArgs args, DateTimeOffset timeOffset) where TJob : IBackgroundJob<TArgs>
        {
            return BackgroundJob.Schedule<TJob>((x) => x.Execute(args), timeOffset);
        }

    }
}
