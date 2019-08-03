using Volo.Abp.Modularity;

namespace Volo.Abp.EntityFrameworkCore.SqlServer
{
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
        )]
    public class AbpEntityFrameworkCoreSqlServerModule : AbpModule
    {
        /// <summary>配置服务</summary>
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
        }
    }
}
