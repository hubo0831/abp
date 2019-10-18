using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    /// <summary>Quartz任务扩展</summary>
    public static class AbpQuartzJobExtensions
    {
        /// <summary>新任务索引</summary>
        private volatile static int m_NewJobIndex;
        /// <summary>调度实时任务</summary>
        public static async Task ScheduleRealTimeJob<TJob>(this IServiceProvider provider, object data = null, Action<ITrigger, IJobDetail> action = null)
            where TJob : IJob
        {
            var scheduler = provider.GetRequiredService<IScheduler>();
            await ScheduleRealTimeJob<TJob>(scheduler, data, action);
        }
        /// <summary>调度实时任务</summary>
        public static async Task ScheduleRealTimeJob<TJob>(this IJobExecutionContext context, object data = null)
            where TJob : IJob
        {
            await ScheduleRealTimeJob<TJob>(context.Scheduler, data);
        }
        /// <summary>调度实时任务</summary>
        public static async Task ScheduleRealTimeJob<TJob>(this IScheduler scheduler, object data = null, Action<ITrigger, IJobDetail> action = null)
            where TJob : IJob
        {
            var newJobIndex = Interlocked.Increment(ref m_NewJobIndex).ToString();
            var name = $"{AbpQuartzConsts.RealTimeJob}:{newJobIndex}";
            var trigger = TriggerBuilder.Create()
                .WithIdentity(name, AbpQuartzConsts.TriggerGroup)
                .StartNow()
                .Build();
            var job = JobBuilder.Create<TJob>()
                .WithIdentity(name, AbpQuartzConsts.JobGroup)
                .UsingJobDataIgnoreIfNull(data)
                .Build();
            action?.Invoke(trigger, job);
            await scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>调度任务</summary>
        public static async Task ScheduleJob<TJob>(this IServiceProvider provider, DateTime scheduleTime, object data = null, Action<ITrigger, IJobDetail> action = null)
            where TJob : IJob
        {
            var scheduler = provider.GetRequiredService<IScheduler>();
            await ScheduleJob<TJob>(scheduler, scheduleTime, data, action);
        }
        /// <summary>调度任务</summary>
        public static async Task ScheduleJob<TJob>(this IScheduler scheduler, DateTime scheduleTime, object data = null, Action<ITrigger, IJobDetail> action = null)
            where TJob : IJob
        {
            var newJobIndex = Interlocked.Increment(ref m_NewJobIndex).ToString();
            var name = $"{AbpQuartzConsts.ScheduleJob}:{newJobIndex}";
            var trigger = TriggerBuilder.Create()
                .WithIdentity(name, AbpQuartzConsts.TriggerGroup)
                .StartAt(scheduleTime.ToUniversalTime())
                .Build();
            var job = JobBuilder.Create<TJob>()
                .WithIdentity(name, AbpQuartzConsts.JobGroup)
                .UsingJobDataIgnoreIfNull(data)
                .Build();
            action?.Invoke(trigger, job);
            await scheduler.ScheduleJob(job, trigger);
        }
        /// <summary>加入任务数据</summary>
        public static JobBuilder UsingJobDataIgnoreIfNull(this JobBuilder jobBuilder, object data = null)
        {
            if (data == null) return jobBuilder;
            return jobBuilder.UsingJobData(new JobDataMap() { ["data"] = data });
        }
        /// <summary>获得任务数据</summary>
        public static TData GetJobData<TData>(this IJobExecutionContext context)
            where TData : class
        {
            return context.JobDetail.JobDataMap.Get("data").As<TData>();
        }
    }
}
