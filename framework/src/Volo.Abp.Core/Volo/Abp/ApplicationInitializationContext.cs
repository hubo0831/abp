using System;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Volo.Abp.DependencyInjection;

namespace Volo.Abp
{
    public class ApplicationInitializationContext : IServiceProviderAccessor
    {
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>默认配置</summary>
        public IConfiguration DefaultConfiguration { get; set; }
        /// <summary>获得默认主机环境</summary>
        public IHostingEnvironment DefaultHostingEnvironment { get; set; }

        public ApplicationInitializationContext([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            ServiceProvider = serviceProvider;
            DefaultConfiguration = serviceProvider.GetRequiredService<IConfiguration>();
            DefaultHostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();
        }
    }
}