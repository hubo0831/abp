using System;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Volo.Abp.Nacos.Configuration
{
    /// <summary>配置扩展</summary>
    public static class ConfigurationExtensions
    {
        /// <summary>加入Nacos配置源</summary>
        public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, IHostBuilder hostBuilder = null, Action<NacosConfigurationSource> configureSource = null)
        {
            var source = new NacosConfigurationSource();
            hostBuilder?.ConfigureServices((_, services) => services.AddSingleton(source));
            configureSource?.Invoke(source);
            return builder.Add(source);
        }
    }
}
