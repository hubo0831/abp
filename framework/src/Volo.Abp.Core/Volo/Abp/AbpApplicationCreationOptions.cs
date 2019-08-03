using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp
{
    public class AbpApplicationCreationOptions
    {
        [NotNull]
        public IServiceCollection Services { get; }

        [NotNull]
        public PlugInSourceList PlugInSources { get; }

        /// <summary>是否使用系统自带的配置</summary>
        public bool UseDefaultConfiguration { get; set; }

        [NotNull]
        public ConfigurationBuilderOptions Configuration { get; }

        public AbpApplicationCreationOptions([NotNull] IServiceCollection services)
        {
            Services = Check.NotNull(services, nameof(services));
            PlugInSources = new PlugInSourceList();
            Configuration = new ConfigurationBuilderOptions();
        }
    }
}