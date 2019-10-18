using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Quartz.HostedService;
using Quartz.Impl;
using Quartz.Spi;

using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;

namespace Volo.Abp.BackgroundJobs.Quartz
{
    /// <summary>Quartz模块</summary>
    [DependsOn(
        typeof(AbpUnitOfWorkModule),
        typeof(AbpDddDomainModule)
        )]
    public class AbpBackgroundJobsQuartzModule : AbpModule
    {
        /// <summary>配置服务</summary>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpQuartzOption>(context.DefaultConfiguration.GetSection(AbpQuartzConsts.SectionName));
            context.Services.AddSingleton<IJobFactory, JobFactory>();
            context.Services.AddSingleton(provider =>
            {
                var option = provider.GetRequiredService<IOptions<AbpQuartzOption>>().Value;
                var schedulerFactory = new StdSchedulerFactory(option.ToProperties());
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = provider.GetService<IJobFactory>();
                return scheduler;
            });

            context.Services.AddHostedService<QuartzHostedService>();
        }
    }
}
