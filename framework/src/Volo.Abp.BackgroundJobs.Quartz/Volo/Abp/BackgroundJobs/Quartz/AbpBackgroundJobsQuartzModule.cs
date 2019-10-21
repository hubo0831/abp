
using Quartz.HostedService;

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
            context.Services.AddQuartzHostedService(context.DefaultConfiguration);
        }
    }
}
