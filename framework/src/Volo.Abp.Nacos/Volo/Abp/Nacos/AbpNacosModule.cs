using Microsoft.Extensions.DependencyInjection;

using Volo.Abp.Modularity;
using Volo.Abp.Nacos.BackgroundServices;
using Volo.Abp.Nacos.Configuration;

namespace Volo.Abp.Nacos
{
    public class AbpNacosModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.Configure<NacosConfigOptions>(context.DefaultConfiguration.GetSection(NacosConfigurationProvider.NacosConfigConfigurationKey));
            context.Services.AddNacos(context.DefaultConfiguration);
            context.Services.AddHostedService<NacosConfigurationListenBackgroundService>();
        }
        /// <summary>预初始化</summary>
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var sp = context.ServiceProvider;
            using (var scope = sp.CreateScope())
            {
                sp = scope.ServiceProvider;
                sp.GetRequiredService<NacosConfigurationSource>().Provider.Load(sp);
            }
        }
    }
}
