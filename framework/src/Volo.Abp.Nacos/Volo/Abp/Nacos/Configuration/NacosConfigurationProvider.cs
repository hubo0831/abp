using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Nacos;

using Volo.Abp.Configuration;
using Volo.Abp.Threading;

namespace Volo.Abp.Nacos.Configuration
{
    /// <summary>Nacos配置提供者</summary>
    public interface INacosConfigurationProvider
    {
        /// <summary>重新加载配置</summary>
        void Reload(string content);
    }

    /// <summary>Nacos配置提供者</summary>
    public class NacosConfigurationProvider : ConfigurationProvider, INacosConfigurationProvider
    {
        /// <summary>配置的配置节</summary>
        public const string NacosConfigConfigurationKey = "nacos.config";
        /// <summary>配置内容</summary>
        private string Content { get; set; }
        /// <summary>加载</summary>
        public void Load(IServiceProvider serviceProvider)
        {
            var client = serviceProvider.GetRequiredService<INacosConfigClient>();
            var options = serviceProvider.GetRequiredService<IOptions<NacosConfigOptions>>().Value;
            var request = new GetConfigRequest()
            {
                Tenant = options.Tenant,
                DataId = options.DataId,
                Group = options.Group
            };
            this.Content = AsyncHelper.RunSync(async () => await client.GetConfigAsync(request));
            Load();
            this.Content = null;
        }
        /// <summary>加载</summary>
        public override void Load()
        {
            if (this.Content.IsNullOrEmpty()) return;
            var jsonConfigurationProvider = new AbpJsonConfigurationProvider();
            this.Data = jsonConfigurationProvider.Load(this.Content);
        }
        /// <summary>重新加载配置</summary>
        public void Reload(string content)
        {
            this.Content = content;
            Load();
            this.Content = null;
            this.OnReload();
        }
    }
}
