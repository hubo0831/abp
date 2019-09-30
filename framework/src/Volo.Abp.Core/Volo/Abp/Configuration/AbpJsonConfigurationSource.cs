using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Volo.Abp.Configuration
{
    /// <summary>JSON配置源</summary>
    public class AbpJsonConfigurationSource : JsonConfigurationSource
    {
        /// <summary>生成</summary>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new AbpJsonConfigurationProvider();
        }
    }
}
