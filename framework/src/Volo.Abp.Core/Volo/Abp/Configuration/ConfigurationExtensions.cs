using System;

using Microsoft.Extensions.Configuration;

namespace Volo.Abp.Configuration
{
    /// <summary>配置扩展</summary>
    public static class ConfigurationExtensions
    {
        /// <summary>加入变量配置源</summary>
        public static IConfigurationBuilder AddVariables(this IConfigurationBuilder builder)
        {
            return builder.Add<VariablesConfigurationSource>(configureSource =>
            {
                configureSource.Configuration = builder.Build();
            });
        }
    }
}
