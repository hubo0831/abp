using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Nacos.Configuration
{
    /// <summary>配置扩展</summary>
    public static class ConfigurationExtensions
    {
        /// <summary>加入Nacos配置源</summary>
        public static IConfigurationBuilder AddNacos(this IConfigurationBuilder builder, IWebHostBuilder hostBuilder = null, Action<NacosConfigurationSource> configureSource = null)
        {
            var source = new NacosConfigurationSource();
            hostBuilder?.ConfigureServices(services => services.AddSingleton(source));
            configureSource?.Invoke(source);
            return builder.Add(source);
        }
    }
}
